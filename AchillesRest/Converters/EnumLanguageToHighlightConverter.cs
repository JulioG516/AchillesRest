using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AchillesRest.Converters;

public class EnumLanguageToHighlightConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
            return "C#";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}