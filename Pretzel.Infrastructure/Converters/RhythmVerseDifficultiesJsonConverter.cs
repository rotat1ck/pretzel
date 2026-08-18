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

        if (root.ValueKind is not JsonValueKind.Object)
        {
            return default;
        }

        foreach (var obj in root.EnumerateObject())
        {
            if (obj.Value.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(obj.Name);
            if (instrumentType is null)
            {
                continue;
            }

            var difficulties = ParseDifficulties(obj.Value);
            if (difficulties is null)
            {
                return default;
            }

            dict[instrumentType.Value] = difficulties;
        }

        return dict.Select(kvp => new ChartInstrument { Instrument = kvp.Key, AvailableDifficulties = kvp.Value });
    }

    private HashSet<DifficultyLevel>? ParseDifficulties(JsonElement obj)
    {
        var difficulties = new HashSet<DifficultyLevel>();
        if (obj.TryGetProperty("all", out var allProperty) &&
            allProperty.TryGetInt32(out int allValue) &&
            allValue == 1)
        {
            difficulties.UnionWith(AllDifficulties);
            return difficulties;
        }

        if (obj.ValueKind is not JsonValueKind.Object)
        {
            return default;
        }

        foreach (var difficulty in obj.EnumerateObject())
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
