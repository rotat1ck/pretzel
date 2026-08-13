namespace Pretzel.Core.Models.Search;

public class ChartSearchAdvancedOptions : ChartSearchOptions
{
    public string? Name { get; set; }
    public string? Album { get; set; }
    public string? Artist { get; set; }
    public string? Charter { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }
}
