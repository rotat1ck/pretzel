namespace Pretzel.Core.Models.Chart.SourceExtensions;

public class RhythmVerseChartSourceExtensionData : ChartSourceExtensionData
{
    /*
     * mark chart as not avaivable for download
     * need time to wrap my head on how to properly handle all of
     * google drive download types, at least - folders and files
    */
    public required bool IsExternal { get; set; }
}