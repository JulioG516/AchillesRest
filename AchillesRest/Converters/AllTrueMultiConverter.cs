using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace AchillesRest.Converters;

public class AllTrueMultiConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Any(v => v == null || !(v is bool)))
            return false;

        return values.All(v => (bool)v);
    }
}