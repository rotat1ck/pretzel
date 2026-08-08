using Pretzel.Core.Enums;

namespace Pretzel.Core.Models;

public class ChartDownloadSource
{
    public ChartSource Source { get; set; }
    public string ChartUri { get; set; } = string.Empty;
    public string? AlbumArtUri { get; set; }
}
