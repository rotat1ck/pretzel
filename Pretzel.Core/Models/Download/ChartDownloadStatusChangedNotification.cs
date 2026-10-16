using MediatR;
using Pretzel.Core.Models.Chart;

namespace Pretzel.Core.Models.Download;

public record ChartDownloadStatusChangedNotification(ChartDownloadSource Source) : INotification { }
