using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AchillesRest.Models;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class MenuCollectionsViewModel : ViewModelBase
{
    public MenuCollectionsViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;

        CreateCollectionCommand = ReactiveCommand.Create(CreateCollection);
        
        // Context Menu action in Collections
        CtxAddRequestCommand = ReactiveCommand.Create<CollectionViewModel>(ContextMenuAddRequest);
        CtxDeleteCollectionCommand = ReactiveCommand.Create<CollectionViewModel>(ContextMenuDeleteCollection);

        // Actions in requests
        CtxDeleteRequestCommand = ReactiveCommand.Create<RequestViewModel>(ContextMenuDeleteRequest);
    }

    public ICommand CtxAddRequestCommand { get; }

    private void ContextMenuAddRequest(CollectionViewModel collectionViewModel)
    {
        collectionViewModel.Requests!.Add(new RequestViewModel(new Request
        {
            Action = EnumActions.GET,
            Name = "Unnamed"
        }));
    }

    public ICommand CtxDeleteRequestCommand { get; }

    private void ContextMenuDeleteRequest(RequestViewModel request)
    {
        var collection = Collections.FirstOrDefault(w => w.Requests != null && w.Requests.Contains(request));

        if (collection != null)
        {
            // Remove the request from the collection
            collection.Requests!.Remove(request);
        }
    }

    public ICommand CtxDeleteCollectionCommand { get; }

    public ICommand CreateCollectionCommand { get; }

    private void CreateCollection()
    {
        Collections.Add(new CollectionViewModel(new Collection
        {
            Name = "Unnamed Collection",
            Requests = new List<Request>()
        }));
    }

    private void ContextMenuDeleteCollection(CollectionViewModel collectionViewModel)
    {
        Collections.Remove(collectionViewModel);
    }


    public ObservableCollection<CollectionViewModel> Collections { get; } = new(FillCollection());

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
                    new() { Action = EnumActions.DELETE, Name = "Teste", Endpoint = "http://localhost:5200/test/get" },
                    new()
                    {
                        Action = EnumActions.OPTIONS, Name = "Teste 2", Endpoint = "http://localhost:5200/test/options"
                    },
                    new() { Action = EnumActions.POST, Name = "Teste 3", Endpoint = "http://localhost:5200/test/post" },
                }
            }),
            new(new Collection
            {
                Name = "Col 2.",
                Requests = new List<Request>
                {
                    new() { Action = EnumActions.HEAD, Name = "Teste 2.1", Endpoint = "http://localhost:5200/test2" },
                    new() { Action = EnumActions.PUT, Name = "Teste 2.2", Endpoint = "http://localhost:5200/test2" },
                    new() { Action = EnumActions.PATCH, Name = "Teste 2.3", Endpoint = "http://localhost:5200/test2" },
                }
            }),
        };
    }


    // private RequestViewModel? _selectedRequest;
    //
    // public RequestViewModel? SelectedRequest
    // {
    //     get => _selectedRequest;
    //     set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    // }
}