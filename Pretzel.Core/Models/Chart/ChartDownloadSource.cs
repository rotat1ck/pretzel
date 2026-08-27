using Pretzel.Core.Enums;
using Pretzel.Core.Models.Chart.SourceExtensions;

namespace Pretzel.Core.Models.Chart;

public class ChartDownloadSource
{
    public ChartSource Source { get; set; }
    public ChartSourceExtensionData? ExtensionData { get; set; }
    public required Uri ChartUri { get; set; }
    public Uri? AlbumArtUri { get; set; }
}
