using System;
using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models.CodeGenerations;

public class CSharpCodeGenerator : CodeGenerator
{
    public override string GenerateCode(EnumActions action, string endpoint,
        string? body, List<KeyValueParamViewModel> headers, IAuthentication? authentication)
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

        string contentString = string.Empty;

        if (body != null && (action == EnumActions.POST
                             || action == EnumActions.PUT
                             || action == EnumActions.PATCH))
        {
            // TODO: Change dynamically via Header Content Type. if  contains but disabled use text/plain.
            // If does not contain use application/json as default.

            string contentType = "application/json";

            contentString = $@"var content = new StringContent(""{FormatBody(body)}"", null, ""{contentType}"");";
        }


        string generatedCode = $@"
var client = new HttpClient();
var requestMessage = new HttpRequestMessage({method}, ""{endpoint}"");";

        if (!string.IsNullOrEmpty(authorizationString))
        {
            generatedCode += $@"
{authorizationString}";
        }

        if (!string.IsNullOrEmpty(headersString))
        {
            generatedCode += $@"
{headersString}";
        }

        if (!string.IsNullOrEmpty(contentString))
        {
            generatedCode += $@"
{contentString}";
        }

        generatedCode += @"

var response = await client.SendAsync(requestMessage);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());";
        return generatedCode;
    }
}