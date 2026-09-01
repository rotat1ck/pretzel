using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Collections.Concurrent;

namespace Pretzel.Infrastructure.Services.Download;

public class ChartDownloadService(IServiceProvider serviceProvider) : IChartDownloadService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    public ConcurrentDictionary<ChartDownloadSource, DownloadResult> Downloads { get; private set; } = new();

    public async Task DownloadAsync(ChartDownloadSource source, Chart chartInfo, CancellationToken cancellationToken = default)
    {
        if (Downloads.TryGetValue(source, out _))
        {
            await RemoveDownloadAsync(source);
        }

        var strategy = serviceProvider.GetRequiredKeyedService<IChartDownloadStrategy>(source.Source);

        var downloadResult = await strategy.StartDownloadAsync(source, cancellationToken);

        if (downloadResult is not null)
        {
            downloadResult.FileName = $"test.sng";

            Downloads.TryAdd(source, downloadResult);
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
