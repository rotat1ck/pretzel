using Pretzel.Core.Models.Chart;

namespace Pretzel.Core.Models.Download;

public class ChartDownloadAddedArgs(ChartDownloadSource source, ChartDownload chartDownload, Chart.Chart chartInfo) : EventArgs
{
    public ChartDownloadSource ChartDownloadSource { get; } = source;
    public ChartDownload ChartDownload { get; } = chartDownload;
    public Chart.Chart Chart { get; } = chartInfo;
}
