using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Collections.Concurrent;

namespace Pretzel.Infrastructure.Services.Download;

public class ChartDownloadService(IServiceProvider serviceProvider,
                                  IChartWriterService writerService) : IChartDownloadService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly IChartWriterService writerService = writerService;

    public event EventHandler<ChartDownloadAddedArgs>? OnChartDownloadAdded;
    public event EventHandler<ChartDownloadRemovedArgs>? OnChartDownloadRemoved;

    public ConcurrentDictionary<ChartDownloadSource, ChartDownload> Downloads { get; private set; } = new();


    public async Task DownloadAsync(ChartDownloadSource source, Chart chartInfo)
    {
        var chartDownload = Downloads.GetOrAdd(source, src =>
        {
            var download = new ChartDownload();
            OnChartDownloadAdded?.Invoke(this, new ChartDownloadAddedArgs(source, download, chartInfo));
            return download;
        });

        lock (chartDownload)
        {
            if (chartDownload.Status is DownloadStatus.Starting or DownloadStatus.Downloading or DownloadStatus.Writing)
            {
                // an exception system or notification for ui?
                return;
            }

            chartDownload.Status = DownloadStatus.Starting;
        }

        var strategy = serviceProvider.GetRequiredKeyedService<IChartDownloadStrategy>(source.Source);

        try
        {
            chartDownload.Status = DownloadStatus.Downloading;
            var result = await strategy.DownloadAsync(source, chartDownload.Cts.Token);

            if (result is not null)
            {
                chartDownload.Result = result;

                chartDownload.Status = DownloadStatus.Writing;
                var status = await writerService.WriteChartAsync(result, chartInfo, chartDownload.Cts.Token);
                chartDownload.Status = status;
            }
        }
        catch (OperationCanceledException)
        {
            /*
             * download strategies aren't handling exceptions
             * if cancellation will be thrown in them, it'll fall downwards to catch block
             * and handled as a Failed download
            */
            chartDownload.Status = DownloadStatus.Cancelled;
        }
        catch
        {
            chartDownload.Status = DownloadStatus.Failed;
        }
        finally
        {
            await chartDownload.DisposeAsync();
        }
    }

    public ChartDownload? GetChartDownload(ChartDownloadSource source)
    {
        if (Downloads.TryGetValue(source, out var chartDownload))
        {
            return chartDownload;
        }

        return default;
    }

    public async Task RemoveDownloadAsync(ChartDownloadSource source)
    {
        if (Downloads.TryRemove(source, out var chartDownload))
        {
            //await chartDownload.Result.DisposeAsync();
            OnChartDownloadRemoved?.Invoke(this, new ChartDownloadRemovedArgs(source));
            await chartDownload.DisposeAsync();
        }
    }
}
