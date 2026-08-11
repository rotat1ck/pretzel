using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.DTOs.Search;

public class ChorusEncoreSearchResponse
{
    [JsonPropertyName("found")]
    public int Count { get; set; }

    [JsonPropertyName("data")]
    public required IEnumerable<ChorusEncoreChartResponse> Items { get; set; }
}