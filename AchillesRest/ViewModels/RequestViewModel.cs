using AchillesRest.Models;
using AchillesRest.Models.Enums;
using ReactiveUI;

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
        set => this.RaiseAndSetIfChanged(ref _endpoint, value);
    }

    public RequestViewModel()
    {
    }

    public RequestViewModel(Request request)
    {
        Name = request.Name;
        Action = request.Action;
        Endpoint = request.Endpoint;
    }
}