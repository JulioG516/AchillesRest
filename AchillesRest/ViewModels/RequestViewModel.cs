﻿using AchillesRest.Models;
using ReactiveUI;

namespace AchillesRest.ViewModels;

public class RequestViewModel : ViewModelBase
{
    private string? _method;

    public string? Method
    {
        get => _method;
        set => this.RaiseAndSetIfChanged(ref _method, value);
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
        Method = request.Method;
        Endpoint = request.Endpoint;
    }
}