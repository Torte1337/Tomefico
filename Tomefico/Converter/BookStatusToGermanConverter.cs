using System;
using Tomefico.Enums;
using Tomefico.Extensions;

namespace Tomefico.Converter;

public class BookStatusToGermanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is BookStatus status)
        {
            return $"({status.ToGerman()})";
        }

        return "(Unbekannt)";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
