using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Search;

public class ChartSearchResults
{
    public required int Count { get; set; }
    public required int Returned { get; set; }
    public required IEnumerable<Chart> Items { get; set; }

    public required ChartSource ChartSource { get; set; }
    public ChartSearchProblemDetails? ProblemDetails { get; set; }

    public class ChartSearchProblemDetails
    {
        public required string? ExceptionName { get; set; }
        public required string? ExceptionMessage { get; set; }
    }
}
