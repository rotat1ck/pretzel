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
        var chartDownload = Downloads.GetOrAdd(source, src => new ChartDownload());
        lock (chartDownload)
        {
            if (chartDownload.Status is DownloadStatus.Starting or DownloadStatus.Downloading or DownloadStatus.Writing)
            {
                // an exception system or notification for ui?
                return;
            }
        }

        // send AddedMessage here later
        await mediator.Publish(new ChartDownloadStatusChangedNotification(source));

        var strategy = serviceProvider.GetRequiredKeyedService<IChartDownloadStrategy>(source.Source);

        try
        {
            var result = await strategy.DownloadAsync(source, chartDownload.Cts.Token);
            await UpdateDownloadAsync(source, chartDownload => chartDownload.Status = DownloadStatus.Downloading);

            if (result is not null)
            {
                await UpdateDownloadAsync(source, chartDownload => chartDownload.Result = result);

                var status = await writerService.WriteChartAsync(result, chartInfo, chartDownload.Cts.Token);
                await UpdateDownloadAsync(source, chartDownload => chartDownload.Status = status);
            }
        }
        catch (Exception ex)
        {
            await UpdateDownloadAsync(source, chartDownload => chartDownload.Status = DownloadStatus.Failed);
        }
    }

    private async Task UpdateDownloadAsync(ChartDownloadSource source, Action<ChartDownload> action)
    {
        var chartDownload = GetChartDownload(source);
        if (chartDownload is null)
        {
            return;
        }
        
        lock (chartDownload)
        {
            action(chartDownload);
        }

        await mediator.Publish(new ChartDownloadStatusChangedNotification(source));
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
            await chartDownload.DisposeAsync();
        }
    }
}
