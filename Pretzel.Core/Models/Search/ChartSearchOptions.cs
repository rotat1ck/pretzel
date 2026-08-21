namespace Pretzel.Core.Models.Search;

public class ChartSearchOptions
{
    public int Page { get; set; } = 1;
    public int Records { get; set; } = DefaultPageSize;

    public string? Search { get; set; }

    public const int DefaultPageSize = 10;
}
