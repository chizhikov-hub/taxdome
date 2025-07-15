using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TaxDome.ShadUI.Common.Localization;

namespace TaxDome.ShadUI.Converters;

public class SelectDocumentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
            return isSelected ? LocalizedStrings.Instance["DocumentHistory_HideSelectionDocuments"] : LocalizedStrings.Instance["DocumentHistory_SelectDocuments"];
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}