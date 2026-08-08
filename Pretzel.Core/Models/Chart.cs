namespace Pretzel.Core.Models;

public class Chart
{
    public string Name { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Charter { get; set; } = string.Empty;

    public List<ChartInstrument> Instruments { get; set; } = new();
    public List<ChartDownloadSource> DownloadSources { get; set; } = new();
}
