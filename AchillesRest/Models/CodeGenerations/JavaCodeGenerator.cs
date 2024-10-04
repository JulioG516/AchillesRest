using System;
using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models.CodeGenerations;

public class JavaCodeGenerator : CodeGenerator
{
    public override string GenerateCode(EnumActions action, string endpoint, List<KeyValueParamViewModel> headers,
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

        string headersString = string.Empty;
        if (headers.Count > 0)
        {
            headersString = string.Join(Environment.NewLine,
                headers.Where(x => x.Enabled).Select(header =>
                    $@"request.setHeader(""{header.Key}"", ""{header.Value}"");"));
        }
        
        string generatedCode = $@"
HttpClient client = HttpClient.newHttpClient();
HttpRequest request = HttpRequest.newBuilder()
    .uri(new URI(""{endpoint}""))
    .method(""{method}"")
    .build();

{headersString}

HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
System.out.println(response.body());
";

        return generatedCode;
    }
}