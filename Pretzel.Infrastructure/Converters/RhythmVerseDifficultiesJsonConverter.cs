using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Converters;

public class RhythmVerseDifficultiesJsonConverter : JsonConverter<IEnumerable<ChartInstrument>>
{
    private static readonly DifficultyLevel[] AllDifficulties = Enum.GetValues<DifficultyLevel>();

    public override IEnumerable<ChartInstrument>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dict = new Dictionary<InstrumentType, HashSet<DifficultyLevel>>();

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        foreach (var obj in root.EnumerateObject())
        {
            InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(obj.Name);
            if (instrumentType is null)
            {
                continue;
            }

            dict[instrumentType.Value] = ParseDifficulties(obj);
        }

        return dict.Select(kvp => new ChartInstrument { Instrument = kvp.Key, AvailableDifficulties = kvp.Value });
    }

    private HashSet<DifficultyLevel> ParseDifficulties(JsonProperty obj)
    {
        var difficulties = new HashSet<DifficultyLevel>();
        if (obj.Value.TryGetProperty("all", out var allProperty) &&
            allProperty.TryGetInt32(out int allValue) &&
            allValue == 1)
        {
            difficulties.UnionWith(AllDifficulties);
            return difficulties;
        }

        foreach (var difficulty in obj.Value.EnumerateObject())
        {
            string instrumentName = difficulty.Name;
            if (!difficulty.Value.TryGetInt32(out int value) || value <= 0)
            {
                continue;
            }

            DifficultyLevel? difficultyLevel = DifficultyLevelResolver.ResolveFromName(instrumentName);
            if (difficultyLevel is null)
            {
                continue;
            }

            difficulties.Add(difficultyLevel.Value);
        }

        return difficulties;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<ChartInstrument> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
