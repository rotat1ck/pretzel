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

    [JsonPropertyName("year")]
    public required string Year { get; set; }


    [JsonPropertyName("md5")]
    public required string ChartUri { get; set; }

    [JsonPropertyName("albumArtMd5")]
    public string? AlbumArtUri { get; set; }


    [JsonPropertyName("notesData")]
    public required NotesData ChartData { get; set; }

    [JsonIgnore]
    public IEnumerable<ChartInstrument> Instruments { get; set; } = [];

    [JsonExtensionData]
    public JsonObject? ExtensionData { get; set; }

    public void OnDeserialized()
    {
        var chartInstruments = JsonSerializer.Deserialize<IEnumerable<ChartInstrument>>(ExtensionData, chartInstrumentsSerializerOptions);
        if (chartInstruments is not null)
        {
            Instruments = chartInstruments;
        }

        foreach (var instrument in Instruments)
        {
            var difficulties = ChartData.Difficulties.FirstOrDefault(ci => ci.Instrument == instrument.Instrument)?.AvailableDifficulties;
            if (difficulties is not null)
            {
                instrument.AvailableDifficulties = difficulties;
            }
        }
    }

    public class NotesData
    {
        [JsonPropertyName("instruments")]
        public required IEnumerable<string> AvailableInstruments { get; set; }

        [JsonPropertyName("noteCounts")]
        [JsonConverter(typeof(ChorusEncoreDifficultiesJsonConverter))]
        public required IEnumerable<ChartInstrument> Difficulties { get; set; }
    }

    private static readonly JsonSerializerOptions chartInstrumentsSerializerOptions = new Lazy<JsonSerializerOptions>(() =>
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ChartInstrumentJsonConverter());
        return options;
    }).Value;
}