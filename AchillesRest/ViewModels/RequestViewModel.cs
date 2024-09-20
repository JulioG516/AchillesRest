using System;
using AchillesRest.Models;
using AchillesRest.Models.Enums;
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