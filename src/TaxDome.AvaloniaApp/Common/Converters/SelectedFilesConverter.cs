using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TaxDome.AvaloniaApp.Common.Converters;

public class SelectedFilesConverter : IMultiValueConverter
{
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is int count && values[1] is string format)
        {
            return string.Format(culture, format, count);
        }

        return string.Empty;
    }
}
