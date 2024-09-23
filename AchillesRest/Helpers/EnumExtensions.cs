using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AchillesRest.Helpers;

public static class EnumExtensions
{
    public static string  GetDescriptionAttribute(this Enum enumValue, Type enumType) 
    {
        string displayName = "";
        MemberInfo? info = enumType.GetMember(enumValue.ToString()).FirstOrDefault();

        if (info != null && info.CustomAttributes.Any())
        {
            DescriptionAttribute? descriptionAttribute = info.GetCustomAttribute<DescriptionAttribute>();
            displayName = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
        }
        else
        {
            displayName = enumValue.ToString();
        }

        return displayName;
    }
}