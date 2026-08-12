namespace Pretzel.Core.Models;

public class Chart
{
    public required string Name { get; set; }
    public required string Album { get; set; }
    public required string Artist { get; set; }
    public required string Charter { get; set; }
    public required string Genre { get; set; }
    public required int Year { get; set; }

    public List<ChartInstrument> Instruments { get; set; } = new();
    public List<ChartDownloadSource> DownloadSources { get; set; } = new();
}
