using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using AchillesRest.ViewModels;
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

    private ObservableCollection<KeyValueParamViewModel>? _headers;

    public ObservableCollection<KeyValueParamViewModel>? Headers
    {
        get => _headers;
        set => this.RaiseAndSetIfChanged(ref _headers, value);
    }

    public bool HasContent => !string.IsNullOrEmpty(Content);
    public bool HasHeaders => Headers?.Count > 0;
}