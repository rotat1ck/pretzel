using Pretzel.Core.Models.Chart;
using System.Globalization;

namespace Pretzel.App.Converters;

public class ChartDownloadSourceTupleValueConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is not [var first, var second])
        {
            return default;
        }

        return (first, second) switch
        {
            (Chart chart, ChartDownloadSource cds) => (chart, cds),
            (ChartDownloadSource cds, Chart chart) => (chart, cds),
            _ => default
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
