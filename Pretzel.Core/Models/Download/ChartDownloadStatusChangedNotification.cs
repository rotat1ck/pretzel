using MediatR;
using Pretzel.Core.Models.Chart;

namespace Pretzel.Core.Models.Download;

public class ChartDownloadStatusChangedNotification : INotification
{
    public ChartDownloadSource Source { get; init; } = null!;
    public Chart.Chart ChartInfo { get; init; } = null!;
}
