namespace Pretzel.Core.Models.Search;

public class ChartSearchResults
{
    public required int Count { get; set; }
    public required int Returned { get; set; }
    public required IEnumerable<Chart> Items { get; set; }
}
