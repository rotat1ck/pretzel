using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Collections.Concurrent;

namespace Pretzel.Core.Interfaces.Download;

public interface IChartDownloadService
{
    ConcurrentDictionary<ChartDownloadSource, DownloadResult> Downloads { get; }

    Task DownloadAsync(ChartDownloadSource source, Chart chartInfo, CancellationToken cancellationToken = default);

    DownloadResult? GetDownloadResult(ChartDownloadSource source);

    Task RemoveDownloadAsync(ChartDownloadSource source);
    Task DisposeDownloadUnmanagedResourcesAsync(ChartDownloadSource source);
}
