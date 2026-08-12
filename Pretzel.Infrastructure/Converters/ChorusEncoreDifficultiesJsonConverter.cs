using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Converters;

public class ChorusEncoreDifficultiesJsonConverter : JsonConverter<IEnumerable<ChartInstrument>>
{
    public override IEnumerable<ChartInstrument>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dict = new Dictionary<InstrumentType, HashSet<DifficultyLevel>>();

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        foreach (var obj in root.EnumerateArray())
        {
            var kvp = ParseInstrumentDifficulty(obj);
            if (!kvp.HasValue)
            {
                continue;
            }

            if (!dict.ContainsKey(kvp.Value.Key))
            {
                dict[kvp.Value.Key] = new();
            }

            dict[kvp.Value.Key].Add(kvp.Value.Value);
        }

        return dict.Select(kvp => new ChartInstrument { AvailableDifficulties = kvp.Value, Instrument = kvp.Key });
    }

    private KeyValuePair<InstrumentType, DifficultyLevel>? ParseInstrumentDifficulty(JsonElement obj)
    {
        var instrumentName = obj.GetProperty("instrument").GetString();
        if (instrumentName is null)
        {
            return default;
        }

        InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(instrumentName);
        if (instrumentType is null)
        {
            return default;
        }

        var instrumentDifficulty = obj.GetProperty("difficulty").GetString();
        if (instrumentDifficulty is null)
        {
            return default;
        }

        DifficultyLevel? difficultyLevel = DifficultyLevelResolver.ResolveFromName(instrumentDifficulty);
        if (difficultyLevel is null)
        {
            return default;
        }

        return new KeyValuePair<InstrumentType, DifficultyLevel>(instrumentType.Value, difficultyLevel.Value);
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<ChartInstrument> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
