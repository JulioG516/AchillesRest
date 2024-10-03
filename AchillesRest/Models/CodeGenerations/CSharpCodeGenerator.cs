﻿using System;
using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models.CodeGenerations;

public class CSharpCodeGenerator : CodeGenerator
{
    public override string GenerateCode(EnumActions action, string endpoint,
        List<KeyValueParamViewModel> headers, IAuthentication? authentication)
    {
        string method = action switch
        {
            EnumActions.GET => "HttpMethod.Get",
            EnumActions.POST => "HttpMethod.Post",
            EnumActions.DELETE => "HttpMethod.Delete",
            EnumActions.HEAD => "HttpMethod.Head",
            EnumActions.PUT => "HttpMethod.Put",
            EnumActions.PATCH => "HttpMethod.Patch",
            EnumActions.OPTIONS => "HttpMethod.Options",
            _ => throw new InvalidOperationException("Invalid Request Action.")
        };

        string authorizationString = string.Empty;
        if (authentication != null)
        {
            authorizationString =
                $@"requestMessage.Headers.Add(""Authorization"", ""{authentication.GetAuthenticationString()}"");";
        }

        string headersString = string.Empty;
        if (headers.Count > 0)
        {
            headersString = string.Join(Environment.NewLine,
                headers.Where(x => x.Enabled).Select(header =>
                    $@"requestMessage.Headers.Add(""{header.Key}"", ""{header.Value}"");"));
        }


        string generatedCode = $@"
var client = new HttpClient();
var requestMessage = new HttpRequestMessage({method}, ""{endpoint}"");

{authorizationString}
{headersString}

var response = await client.SendAsync(requestMessage);";

        return generatedCode;
    }
}