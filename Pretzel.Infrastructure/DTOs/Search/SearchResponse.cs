using Pretzel.Core.Models;

namespace Pretzel.Infrastructure.DTOs.Search;

public class SearchResponse
{
    public required int Count { get; set; }
    public required int Returned { get; set; }
    public required IEnumerable<Chart> Items { get; set; }
}
