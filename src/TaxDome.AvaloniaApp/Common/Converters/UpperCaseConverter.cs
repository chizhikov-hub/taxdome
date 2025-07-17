using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TaxDome.AvaloniaApp.Common.Converters;

public class UpperCaseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as string)?.ToUpper() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}