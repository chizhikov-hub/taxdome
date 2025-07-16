using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TaxDome.AvaloniaApp.Common.Converters;

public class FileSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ulong? fileSize = null;
        if (value is ulong size)
            fileSize = size;
        if (value is long longSize)
            fileSize = (ulong) longSize;
        
        if (fileSize.HasValue)
        {            
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double len = fileSize.Value;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        return "0 B";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}