using System.Collections.Generic;
using System.Collections.ObjectModel;
using AchillesRest.Models;
using ReactiveUI;

namespace AchillesRest.ViewModels;

public class MenuCollectionsViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "CollectionViewModel";
    public IScreen HostScreen { get; }

    public MenuCollectionsViewModel() { }

    public MenuCollectionsViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public ObservableCollection<CollectionViewModel> Collections { get; } = new(FillCollection());

    private Request? _selectedRequest;

    public Request? SelectedRequest
    {
        get => _selectedRequest;
        set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    }

    private static List<CollectionViewModel> FillCollection()
    {
        return new List<CollectionViewModel>
        {
            new(new Collection
            {
                Name = "Col 1.",
                Requests = new List<Request>
                {
                    new() { Method = "GET", Name = "Teste" },
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