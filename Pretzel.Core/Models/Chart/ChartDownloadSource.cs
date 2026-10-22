using Pretzel.Core.Enums;
using Pretzel.Core.Models.Chart.SourceExtensions;

namespace Pretzel.Core.Models.Chart;

public record ChartDownloadSource
{
    public ChartSource Source { get; init; }
    public ChartSourceExtensionData? ExtensionData { get; init; }
    public required Uri ChartUri { get; init; }
    public Uri? AlbumArtUri { get; init; }

    public virtual bool Equals(ChartDownloadSource? other)
    {
        return other is not null
            && Source == other.Source
            && ChartUri == other.ChartUri;
    }
}
