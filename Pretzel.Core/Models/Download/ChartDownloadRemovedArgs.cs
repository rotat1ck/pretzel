using Pretzel.Core.Models.Chart;

namespace Pretzel.Core.Models.Download;

public class ChartDownloadRemovedArgs(ChartDownloadSource source) : EventArgs
{
    public ChartDownloadSource ChartDownloadSource { get; } = source;
}
