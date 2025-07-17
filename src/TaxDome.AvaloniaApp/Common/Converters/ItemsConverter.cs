using System;
using System.Globalization;
using Avalonia.Collections;
using Avalonia.Data.Converters;

namespace TaxDome.AvaloniaApp.Common.Converters;

public class ItemsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DataGridCollectionViewGroup @group)
            return @group.Items;
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
