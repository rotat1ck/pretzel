using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Converters;

public class ChartInstrumentJsonConverter : JsonConverter<IEnumerable<ChartInstrument>>
{
    public override IEnumerable<ChartInstrument>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var obj = JsonObject.Parse(ref reader) as JsonObject;
        var chartInstruments = new List<ChartInstrument>();

        if (obj is not null)
        {
            var definedInstruments = Enum.GetNames<InstrumentType>();
            foreach (var instrument in definedInstruments)
            {
                var chartInstrument = ConvertToChartInstrument(obj, "diff_" + instrument.ToLower());
                if (chartInstrument is not null)
                {
                    chartInstruments.Add(chartInstrument);
                }
            }
        }

        return chartInstruments;
    }

    private ChartInstrument? ConvertToChartInstrument(JsonObject obj, string propertyName)
    {
        InstrumentType? instrumentType = InstrumentTypeResolver.ResolveFromName(propertyName.TrimStart("diff_").ToString());
        if (instrumentType is null)
        {
            return default;
        }

        var instrumentRating = obj[propertyName]?.GetValue<int>();
        if (instrumentRating is null)
        {
            return default;
        }

        return new ChartInstrument
        {
            Instrument = (InstrumentType)instrumentType,
            Rating = (int)instrumentRating
        };
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<ChartInstrument> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
