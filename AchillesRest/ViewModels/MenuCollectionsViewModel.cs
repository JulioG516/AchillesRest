using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using AchillesRest.Models;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class MenuCollectionsViewModel : ViewModelBase
{
    // , IRoutableViewModel
    // public string? UrlPathSegment { get; } = "CollectionViewModel";
    // public IScreen HostScreen { get; }

    public MenuCollectionsViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;
        TestCommand = ReactiveCommand.Create(Test);

        // this.WhenAnyValue(x => x.SelectedRequest)
        //     .Where(x => x is not null)
        //     .Subscribe(x => { RequestService.SelectedRequest = x; });
    }

    public MenuCollectionsViewModel(IScreen hostScreen)
    {
        // HostScreen = hostScreen;
        RequestService = Locator.Current.GetService<RequestService>()!;
        TestCommand = ReactiveCommand.Create(() => { Debug.WriteLine(RequestService.SelectedRequest); });
    }

    public ICommand TestCommand { get; }

    private void Test()
    {
        Debug.WriteLine(RequestService.SelectedRequest);
    }

    public ObservableCollection<CollectionViewModel> Collections { get; } = new(FillCollection());

    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    }

    public RequestService RequestService { get; }


    private static List<CollectionViewModel> FillCollection()
    {
        return new List<CollectionViewModel>
        {
            new(new Collection
            {
                Name = "Col 1.",
                Requests = new List<Request>
                {
                    new() { Method = "GET", Name = "Teste", Endpoint = "http://localhost:5200/test"},
                    new() { Method = "GET", Name = "Teste 2" },
                    new() { Method = "POST", Name = "Teste 3" },
                }
            }),
            new(new Collection
            {
                Name = "Col 2.",
                Requests = new List<Request>
                {
                    new() { Method = "PATCH", Name = "Teste 2.1" },
                    new() { Method = "PUT", Name = "Teste 2.2" },
                    new() { Method = "HEAD", Name = "Teste 2.3" },
                }
            }),
        };
    }
}