using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.DTOs.Search;

public class RhythmVerseSearchResponse
{
    [JsonPropertyName("data")]
    public required ResponseData Data { get; set; }

    public class ResponseData
    {
        [JsonPropertyName("records")]
        public required RecordsData Records { get; set; }

        [JsonPropertyName("songs")]
        public required IEnumerable<RhythmVerseChartResponse> Songs { get; set; }

        public class RecordsData
        {
            [JsonPropertyName("returned")]
            public required int Returned { get; set; }

            [JsonPropertyName("total_filtered")]
            public required int Count { get; set; }
        }
    }
}
