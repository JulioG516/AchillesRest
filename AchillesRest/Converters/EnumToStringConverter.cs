using System;
using System.Globalization;
using AchillesRest.Helpers;
using AchillesRest.Models.Enums;
using Avalonia.Data.Converters;

namespace AchillesRest.Converters;

public class EnumToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        if (value.GetType().IsEnum)
        {
            return ((Enum)value).GetDescriptionAttribute(typeof(EnumAuthTypes));
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return EnumActions.GET; // Default value or handle appropriately

        return Enum.Parse(typeof(EnumActions), value.ToString() ?? string.Empty);
    }
}