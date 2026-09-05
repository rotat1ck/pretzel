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

    public ConcurrentDictionary<ChartDownloadSource, DownloadResult> Downloads { get; private set; } = new();

    public async Task DownloadAsync(ChartDownloadSource source, Chart chartInfo, CancellationToken cancellationToken = default)
    {
        if (Downloads.TryGetValue(source, out _))
        {
            return;
        }

        var downloadResult = new DownloadResult()
        {
            Cts = new CancellationTokenSource()
        };
        Downloads.TryAdd(source, downloadResult);

        // send StatusChangedMessage here later

        var strategy = serviceProvider.GetRequiredKeyedService<IChartDownloadStrategy>(source.Source);

        try
        {
            var result = await strategy.DownloadAsync(source, downloadResult.Cts.Token);
            downloadResult.Status = result?.Status ?? DownloadStatus.Failed;

            if (result is not null)
            {
                // cts is owned and disposed by downloadResult 
                result.Cts = downloadResult.Cts;
                result.Status = DownloadStatus.Downloading;
                Downloads[source] = result;

                // send StatusChangedMessage here later

                result.Status = await writerService.WriteChartAsync(result, chartInfo, result.Cts.Token);
            }
        }
        catch
        {
            downloadResult.Status = DownloadStatus.Failed;
            // send StatusChangedMessage here later
        }


    }

    public DownloadResult? GetDownloadResult(ChartDownloadSource source)
    {
        if (Downloads.TryGetValue(source, out var downloadResult))
        {
            return downloadResult;
        }

        return default;
    }

    public async Task RemoveDownloadAsync(ChartDownloadSource source)
    {
        if (Downloads.TryRemove(source, out var downloadResult))
        {
            await downloadResult.DisposeAsync();
        }
    }

    public async Task DisposeDownloadUnmanagedResourcesAsync(ChartDownloadSource source)
    {
        if (Downloads.TryGetValue(source, out var downloadResult))
        {
            await downloadResult.FreeUnmanagedResourcesAsync();
        }
    }
}
