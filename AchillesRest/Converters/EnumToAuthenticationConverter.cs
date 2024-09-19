using System;
using System.Globalization;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using Avalonia.Data.Converters;

namespace AchillesRest.Converters;

public class EnumToAuthenticationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return EnumAuthTypes.None;

        if (value is IAuthentication auth)
        {
            return auth switch
            {
                BasicAuthentication => EnumAuthTypes.Basic,
                BearerAuthentication => EnumAuthTypes.Bearer,
                DigestAuthentication => EnumAuthTypes.Digest,
                _ => throw new ArgumentException("Invalid authentication type")
            };
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is EnumAuthTypes authType)
        {
            return authType switch
            {
                EnumAuthTypes.None => null,
                EnumAuthTypes.Basic => new BasicAuthentication(),
                EnumAuthTypes.Bearer => new BearerAuthentication(),
                EnumAuthTypes.Digest => new DigestAuthentication(),
                _ => throw new ArgumentException("Invalid authentication type.")
            };
        }

        return null;
    }
}