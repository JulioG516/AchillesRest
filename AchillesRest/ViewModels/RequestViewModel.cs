using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AchillesRest.Models;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace AchillesRest.ViewModels;

public class RequestViewModel : ViewModelBase
{
    private EnumActions? _action;

    public EnumActions? Action
    {
        get => _action;
        set => this.RaiseAndSetIfChanged(ref _action, value);
    }

    private string? _name;

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string? _endpoint;

    public string? Endpoint
    {
        get => _endpoint;
        set { this.RaiseAndSetIfChanged(ref _endpoint, value); }
        //=> this.RaiseAndSetIfChanged(ref _endpoint, value);
    }

    private string? _queryEndpoint;

    public string? QueryEndpoint
    {
        get => _queryEndpoint;
        set => this.RaiseAndSetIfChanged(ref _queryEndpoint, value);
    }

    private string? _generatedCode;

    public string? GeneratedCode
    {
        get => _generatedCode;
        set => this.RaiseAndSetIfChanged(ref _generatedCode, value);
    }

    private void UpdateGeneratedCode()
    {
        string method = Action switch
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

        GeneratedCode = $@"
                var client = new HttpClient();
                var requestMessage = new HttpRequestMessage({method}, ""{Endpoint}"");
                
                var response = await client.SendAsync(requestMessage);";
    }


    private IAuthentication? _authentication;

    public IAuthentication? Authentication
    {
        get => _authentication;
        set => this.RaiseAndSetIfChanged(ref _authentication, value);
    }

    private EnumAuthTypes? _selectedAuthType;

    public EnumAuthTypes? SelectedAuthType
    {
        get => _selectedAuthType;
        set => this.RaiseAndSetIfChanged(ref _selectedAuthType, value);
    }

    private string? _body;

    public string? Body
    {
        get => _body;
        set => this.RaiseAndSetIfChanged(ref _body, value);
    }

    public ObservableCollection<KeyValueParamViewModel> Headers { get; set; } = new();

    public ObservableCollection<KeyValueParamViewModel> QueryParams { get; set; } = new();

    public RequestViewModel()
    {
    }

    public RequestViewModel(Request request)
    {
        Name = request.Name;
        Action = request.Action;
        Endpoint = request.Endpoint;
        SelectedAuthType = request.SelectedAuthType;
        Authentication = request.Authentication;
        Body = request.Body;
        Headers = new ObservableCollection<KeyValueParamViewModel>(
            request.Headers.Select(h => new KeyValueParamViewModel(h)));

        QueryParams = new ObservableCollection<KeyValueParamViewModel>(
            request.QueryParams.Select(q => new KeyValueParamViewModel(q)));


        this.WhenAnyValue(x => x.Action, x
                => x.Endpoint)
            .Subscribe(_ => UpdateGeneratedCode());

        QueryParams
            .ToObservableChangeSet()
            .AutoRefreshOnObservable(kvp => kvp.WhenAnyValue(x => x.Key, x => x.Value, x => x.Enabled))
            .Throttle(TimeSpan.FromMilliseconds(5))
            .Subscribe(_ => UpdateEndpointQueryParams());
    }

    private void UpdateEndpointQueryParams()
    {
        if (!string.IsNullOrEmpty(Endpoint))
        {
            Endpoint = RemoveQueryParams(Endpoint); // Reset to original endpoint if no query params
        }

        if (QueryParams.Count == 0)
        {
            return;
        }

        var queryString = string.Join("&",
            QueryParams.Where(kvp => (!string.IsNullOrEmpty(kvp.Key) || !string.IsNullOrEmpty(kvp.Value))
                                     && kvp.Enabled)
                .Select(kvp => $"{kvp.Key}={kvp.Value}"));

        if (string.IsNullOrEmpty(queryString))
        {
            if (Endpoint != null)
                Endpoint = RemoveQueryParams(Endpoint); // Reset to original endpoint if no valid query params
        }
        else
        {
            if (Endpoint != null)
                Endpoint = RemoveQueryParams(Endpoint) + (Endpoint.Contains("?") ? "&" : "?") + queryString;
        }
    }

    private string RemoveQueryParams(string url)
    {
        int index = url.IndexOf("?");
        return index == -1 ? url : url.Substring(0, index);
    }

    protected bool Equals(RequestViewModel other)
    {
        return _action == other._action && _name == other._name && _endpoint == other._endpoint;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RequestViewModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_action, _name, _endpoint);
    }
}