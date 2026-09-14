using Pretzel.Core.Enums;
using Pretzel.Core.Models.Download;

namespace Pretzel.Core.Interfaces.Download;

public interface IChartWriterStrategy
{
    DownloadFileType FileType { get; }
    Task WriteChartAsync(DownloadResult downloadResult, string targetDirectory, string chartName, CancellationToken cancellationToken);
}
