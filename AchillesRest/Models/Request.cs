using System;
using System.Collections.Generic;
using System.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;

namespace AchillesRest.Models;

public class Request
{
    public string? Name { get; set; }
    public EnumActions? Action { get; set; }
    public string? Endpoint { get; set; }
    public EnumAuthTypes? SelectedAuthType { get; set; } = EnumAuthTypes.None;
    public IAuthentication? Authentication { get; set; }

    public List<KeyValueParam> Headers { get; set; } = new();

    public List<KeyValueParam> QueryParams { get; set; } = new();

    public SupportedLanguagesGeneration? SelectedLanguagesGeneration { get; set; } =
        SupportedLanguagesGeneration.CSharp;

    public string? Body { get; set; }

    public Request()
    {
    }

    public Request(RequestViewModel requestViewModel)
    {
        Name = requestViewModel.Name;
        Action = requestViewModel.Action;
        Endpoint = requestViewModel.Endpoint;
        SelectedAuthType = requestViewModel.SelectedAuthType;
        Authentication = requestViewModel.Authentication;
        Headers = requestViewModel.Headers.Select(h => new KeyValueParam(h)).ToList();
        QueryParams = requestViewModel.QueryParams.Select(h => new KeyValueParam(h)).ToList();
        SelectedLanguagesGeneration = requestViewModel.SelectedLanguageGeneration;
    }
}