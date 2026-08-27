using Pretzel.Core.Models.Chart;
using Pretzel.Infrastructure.Converters;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.DTOs.Search.RhythmVerse;

public class RhythmVerseChartResponse : IJsonOnDeserialized
{
    [JsonPropertyName("data")]
    public required RhythmVerseChartDataResponse Data { get; set; }

    [JsonPropertyName("file")]
    public required RhythmVerseChartFileResponse File { get; set; }


    [JsonIgnore]
    public IEnumerable<ChartInstrument> Instruments { get; set; } = [];

    public void OnDeserialized()
    {
        var chartInstruments = JsonSerializer.Deserialize<IEnumerable<ChartInstrument>>(File.ExtensionData, chartInstrumentsSerializerOptions);
        if (chartInstruments is not null)
        {
            Instruments = chartInstruments;
        }

        foreach (var instrument in Instruments)
        {
            var difficulties = File.Difficulties.FirstOrDefault(ci => ci.Instrument == instrument.Instrument)?.AvailableDifficulties;
            if (difficulties is not null)
            {
                instrument.AvailableDifficulties = difficulties;
            }
        }
    }

    public class RhythmVerseChartDataResponse
    {
        [JsonPropertyName("title")]
        public required string Name { get; set; }

        [JsonPropertyName("album")]
        public required string Album { get; set; }

        [JsonPropertyName("artist")]
        public required string Artist { get; set; }

        [JsonPropertyName("genre")]
        public required string Genre { get; set; }

        [JsonPropertyName("year")]
        public required int Year { get; set; }
    }

    public class RhythmVerseChartFileResponse
    {
        [JsonPropertyName("album_art")]
        public required string? AlbumArtUri { get; set; }

        [JsonPropertyName("download_url")]
        public required string ChartUri { get; set; }

        [JsonPropertyName("author")]
        public required AuthorData Author { get; set; }

        [JsonPropertyName("difficulties")]
        [JsonConverter(typeof(RhythmVerseDifficultiesJsonConverter))]
        public IEnumerable<ChartInstrument> Difficulties { get; set; }

        [JsonPropertyName("external_url")]
        public string? ExternalUrl { get; set; }

        [JsonExtensionData]
        public JsonObject? ExtensionData { get; set; }

        public class AuthorData
        {
            [JsonPropertyName("name")]
            public required string Charter { get; set; }
        }
    }

    private static readonly JsonSerializerOptions chartInstrumentsSerializerOptions = new JsonSerializerOptions
    {
        Converters = { new ChartInstrumentJsonConverter() }
    };
}
