using Pretzel.Infrastructure.DTOs.Search.RhythmVerse;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Converters;

public class RhythmVerseSongsJsonConverter : JsonConverter<IEnumerable<RhythmVerseChartResponse>>
{
    public override IEnumerable<RhythmVerseChartResponse>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
        {
            return new List<RhythmVerseChartResponse>();
        }

        return JsonSerializer.Deserialize<List<RhythmVerseChartResponse>>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<RhythmVerseChartResponse> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
