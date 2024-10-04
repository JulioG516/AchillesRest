using System;
using System.Collections.Generic;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models.CodeGenerations;

public abstract class CodeGenerator
{
    public abstract string GenerateCode(EnumActions action, string endpoint,
        string? body, List<KeyValueParamViewModel> headers, IAuthentication? authentication);

    public string FormatBody(string body)
    {
        return body.Replace("\"", "\\\"")
            .Replace(Environment.NewLine, "")
            .Replace("\n", "").Replace("\r", "");
    }
}