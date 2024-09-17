using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace AchillesRest.Converters;

public class MyMultiValueConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values?.Count == 0)
            throw new NotSupportedException();

        return values.ToArray();
    }
}