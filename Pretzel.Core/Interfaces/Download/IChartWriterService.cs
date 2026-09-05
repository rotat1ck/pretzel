using Pretzel.Core.Enums;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;

namespace Pretzel.Core.Interfaces.Download;

public interface IChartWriterService
{
    Task<DownloadStatus> WriteChartAsync(DownloadResult downloadResult, Chart chartInfo, CancellationToken cancellationToken);
}
