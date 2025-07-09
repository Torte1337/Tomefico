using System;
using System.Globalization;

namespace Tomefico.Converter;

public class SelectedToEnabledConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string current && parameter is string selected)
        {
            return !string.Equals(current, selected, StringComparison.OrdinalIgnoreCase);
        }

        return true; // Button bleibt aktiv, wenn Vergleich fehlschlägt
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Brauchst du nicht
    }    
}
