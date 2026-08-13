using Pretzel.Core.Enums;

namespace Pretzel.Core.Models;

public class ChartDownloadSource
{
    public ChartSource Source { get; set; }
    public ChartSourceExtensionData? ExtensionData { get; set; }
    public required string ChartUri { get; set; }
    public string? AlbumArtUri { get; set; }
}
