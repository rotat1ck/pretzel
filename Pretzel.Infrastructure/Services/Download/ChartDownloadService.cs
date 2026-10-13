using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Collections.Concurrent;

namespace Pretzel.Infrastructure.Services.Download;

public class ChartDownloadService(IServiceProvider serviceProvider,
                                  IChartWriterService writerService,
                                  IMediator mediator) : IChartDownloadService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly IChartWriterService writerService = writerService;
    private readonly IMediator mediator = mediator;

    public ConcurrentDictionary<ChartDownloadSource, ChartDownload> Downloads { get; private set; } = new();

    public async Task DownloadAsync(ChartDownloadSource source, Chart chartInfo)
    {
        if (Downloads.TryGetValue(source, out var existingDownload) 
            && existingDownload.Status is DownloadStatus.Starting or DownloadStatus.Downloading or DownloadStatus.Writing)
        {
            return;
        }

        var chartDownload = new ChartDownload();
        Downloads.TryAdd(source, chartDownload);

        // send StatusChangedMessage here later
        await NotifyStatusChangedAsync(source, chartInfo);

        var strategy = serviceProvider.GetRequiredKeyedService<IChartDownloadStrategy>(source.Source);

        try
        {
            var result = await strategy.DownloadAsync(source, chartDownload.Cts.Token);
            chartDownload.Status = DownloadStatus.Downloading;

            if (result is not null)
            {
                // cts is owned and disposed by chartDownload 
                //result.Cts = downloadResult.Cts;
                //result.Status = DownloadStatus.Downloading;
                //Downloads[source] = result;
                chartDownload.Result = result;


                // send StatusChangedMessage here later
                await NotifyStatusChangedAsync(source, chartInfo);

                chartDownload.Status = await writerService.WriteChartAsync(result, chartInfo, chartDownload.Cts.Token);
                await NotifyStatusChangedAsync(source, chartInfo);
            }
        }
        catch (Exception ex)
        {
            chartDownload.Status = DownloadStatus.Failed;
            // send StatusChangedMessage here later
            await NotifyStatusChangedAsync(source, chartInfo);
        }
    }

    private async Task NotifyStatusChangedAsync(ChartDownloadSource source, Chart chartInfo)
    {
        await mediator.Publish(new ChartDownloadStatusChangedNotification
        {
            Source = source,
            ChartInfo = chartInfo
        });
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
            await chartDownload.Result.DisposeAsync();
            chartDownload.Dispose();
        }
    }

    public async Task DisposeDownloadUnmanagedResourcesAsync(ChartDownloadSource source)
    {
        if (Downloads.TryGetValue(source, out var chartDownload))
        {
            await chartDownload.Result.FreeUnmanagedResourcesAsync();
            chartDownload.Dispose();
        }
    }
}
