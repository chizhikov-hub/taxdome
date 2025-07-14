using System.Globalization;
using System.Windows.Data;

namespace TaxDome.Presentation.Common.Converters;

public class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.ToString("G", CultureInfo.CurrentCulture);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
