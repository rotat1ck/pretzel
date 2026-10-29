using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Collections.Concurrent;

namespace Pretzel.Core.Interfaces.Download;

public interface IChartDownloadService
{
    ConcurrentDictionary<ChartDownloadSource, ChartDownload> Downloads { get; }
    event EventHandler<ChartDownloadAddedArgs>? OnChartDownloadAdded;
    event EventHandler<ChartDownloadRemovedArgs>? OnChartDownloadRemoved;

    Task DownloadAsync(ChartDownloadSource source, Chart chartInfo);

    ChartDownload? GetChartDownload(ChartDownloadSource source);

    Task RemoveDownloadAsync(ChartDownloadSource source);
}
