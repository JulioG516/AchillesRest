using System;
using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models.CodeGenerations;

public class JavaCodeGenerator : CodeGenerator
{
    public override string GenerateCode(EnumActions action, string endpoint,
        string? body, List<KeyValueParamViewModel> headers,
        IAuthentication? authentication)
    {
        string method = action switch
        {
            EnumActions.GET => "GET",
            EnumActions.POST => "POST",
            EnumActions.DELETE => "DELETE",
            EnumActions.HEAD => "HEAD",
            EnumActions.PUT => "PUT",
            EnumActions.PATCH => "PATCH",
            EnumActions.OPTIONS => "OPTIONS",
            _ => throw new InvalidOperationException("Invalid Request Action.")
        };


        string setAuthentication = string.Empty;
        if (authentication != null)
        {
            setAuthentication = $@".setHeader(""Authorization"", ""{authentication.GetAuthenticationString()}"")";
        }


        string setHeaders = string.Empty;
        if (headers.Count > 0)
        {
            setHeaders = string.Join(Environment.NewLine,
                headers.Where(x => x.Enabled).Select(header =>
                    $@".setHeader(""{header.Key}"", ""{header.Value}"")"));
        }

        string methodPublisher = "HttpRequest.BodyPublishers.noBody()";
        if (!string.IsNullOrEmpty(body) &&
            (action == EnumActions.POST
             || action == EnumActions.PUT
             || action == EnumActions.PATCH))
        {
            string escapedBody = FormatBody(body);
            methodPublisher = $@"HttpRequest.BodyPublishers.ofString(""{escapedBody}"")";
        }

        string generatedCode = $@"
HttpClient client = HttpClient.newHttpClient();
HttpRequest request = HttpRequest.newBuilder()
.uri(new URI(""{endpoint}""))
.method(""{method}"", {methodPublisher})";

        if (!string.IsNullOrEmpty(setAuthentication))
        {
            generatedCode += @$"
{setAuthentication}";
        }

        if (!string.IsNullOrEmpty(setHeaders))
        {
            generatedCode += @$"
{setHeaders}";
        }


        generatedCode += @"
.build();

HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
System.out.println(response.body()); ";

        return generatedCode;
    }
}