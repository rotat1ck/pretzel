using Pretzel.Core.Enums;
using Pretzel.Core.Models.Chart;
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

        if (root.ValueKind is not JsonValueKind.Array)
        {
            return default;
        }

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
        if (!obj.TryGetProperty("instrument", out var instrumentProperty) || instrumentProperty.ValueKind is not JsonValueKind.String)
        {
            return default;
        }

        InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(instrumentProperty.GetString()!);
        if (instrumentType is null)
        {
            return default;
        }

        if (!obj.TryGetProperty("difficulty", out var difficultyProperty) || difficultyProperty.ValueKind is not JsonValueKind.String)
        {
            return default;
        }

        DifficultyLevel? difficultyLevel = DifficultyLevelResolver.ResolveFromName(difficultyProperty.GetString()!);
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
