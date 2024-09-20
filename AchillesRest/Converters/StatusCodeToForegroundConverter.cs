using System;
using System.Globalization;
using System.Net;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AchillesRest.Converters;

public class StatusCodeToForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.OK => Brushes.Green,
                HttpStatusCode.Accepted => Brushes.Green,
                _ => Brushes.Red
            };
        }

        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}