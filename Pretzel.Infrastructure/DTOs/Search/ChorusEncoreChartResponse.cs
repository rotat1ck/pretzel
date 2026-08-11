using Pretzel.Core.Models;
using Pretzel.Infrastructure.Converters;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.DTOs.Search;

public class ChorusEncoreChartResponse : IJsonOnDeserialized
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("album")]
    public required string Album { get; set; }

    [JsonPropertyName("artist")]
    public required string Artist { get; set; }

    [JsonPropertyName("charter")]
    public required string Charter { get; set; }

    [JsonPropertyName("genre")]
    public string? Genre { get; set; }


    [JsonPropertyName("md5")]
    public required string ChartUri { get; set; }

    [JsonPropertyName("albumArtMd5")]
    public string? AlbumArtUri { get; set; }


    [JsonPropertyName("notesData")]
    public required NotesData ChartData { get; set; }

    [JsonIgnore]
    public IEnumerable<ChartInstrument> Instruments { get; set; } = [];

    [JsonExtensionData]
    public JsonObject ExtensionData { get; set; }

    public void OnDeserialized()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ChartInstrumentJsonConverter());

        // first priority - rating
        var chartInstruments = JsonSerializer.Deserialize<IEnumerable<ChartInstrument>>(ExtensionData, options);
        if (chartInstruments is not null)
        {
            Instruments = chartInstruments;
        }

        // difficulties
        foreach (var instrument in Instruments)
        {
            var difficulties = ChartData.DifficultyData.FirstOrDefault(ci => ci.Instrument == instrument.Instrument)?.AvailableDifficulties;
            if (difficulties is null)
            {
                continue;
            }

            instrument.AvailableDifficulties = difficulties;
        }
    }

    public class NotesData
    {
        [JsonPropertyName("instruments")]
        public required IEnumerable<string> AvailableInstruments { get; set; }

        [JsonPropertyName("noteCounts")]
        [JsonConverter(typeof(ChorusEncoreDifficultiesJsonConverter))]
        public required IEnumerable<ChartInstrument> DifficultyData { get; set; }
    }
}