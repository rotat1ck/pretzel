using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Converters;

public class ChorusEncoreDifficultiesJsonConverter : JsonConverter<IEnumerable<ChartInstrument>>
{
    public override IEnumerable<ChartInstrument>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dict = new Dictionary<InstrumentType, HashSet<DifficultyLevel>>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            var obj = JsonObject.Parse(ref reader) as JsonObject;
            if (obj is null)
            {
                continue;
            }

            var kvp = ConvertToKeyValuePair(obj);
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

    private KeyValuePair<InstrumentType, DifficultyLevel>? ConvertToKeyValuePair(JsonObject obj)
    {
        var instrumentName = obj["instrument"]?.GetValue<string>();
        if (instrumentName is null)
        {
            return default;
        }

        InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(instrumentName);
        if (instrumentType is null)
        {
            return default;
        }

        var instrumentDifficulty = obj["difficulty"]?.GetValue<string>();
        if (instrumentDifficulty is null)
        {
            return default;
        }

        DifficultyLevel? difficultyLevel = DifficultyLevelResolver.ResolveFromName(instrumentDifficulty);
        if (difficultyLevel is null)
        {
            return default;
        }

        return new KeyValuePair<InstrumentType, DifficultyLevel>((InstrumentType)instrumentType, (DifficultyLevel)difficultyLevel);
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<ChartInstrument> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
