using System.Net.Http;
using ReactiveUI;

namespace AchillesRest.Models;

/// <summary>
/// Encapsulate the HttpResponse and Content a as string to be used in the dictionary of requests. 
/// </summary>
public class AchillesHttpResponse : ReactiveObject
{
    private HttpResponseMessage? _HttpMessage;
    public HttpResponseMessage? HttpMessage
    {
        get => _HttpMessage;
        set => this.RaiseAndSetIfChanged(ref _HttpMessage, value);
    }

    private string? _content;
    public string? Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    public bool HasContent => !string.IsNullOrEmpty(Content);
}