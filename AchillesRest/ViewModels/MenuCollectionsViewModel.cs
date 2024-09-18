using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Windows.Input;
using AchillesRest.Models;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using DynamicData;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class MenuCollectionsViewModel : ViewModelBase, IDisposable
{
    public MenuCollectionsViewModel()
    {
        _sourceList.AddRange(FillCollection());

        RequestService = Locator.Current.GetService<RequestService>()!;

        CreateCollectionCommand = ReactiveCommand.Create(CreateCollection);

        // Context Menu action in Collections
        CtxAddRequestCommand = ReactiveCommand.Create<CollectionViewModel>(ContextMenuAddRequest);
        CtxDeleteCollectionCommand = ReactiveCommand.Create<CollectionViewModel>(ContextMenuDeleteCollection);
        CtxCollectionPropertiesCommand =
            ReactiveCommand.Create<CollectionViewModel>(ContextMenuSeeCollectionProperties);

        // Actions in requests
        CtxDeleteRequestCommand = ReactiveCommand.Create<RequestViewModel>(ContextMenuDeleteRequest);

        // Select in the shared properties an value that can be RequestViewModel or CollectionViewModel.
        this.WhenAnyValue(x => x.SelectedNode)
            .DistinctUntilChanged()
            .Where(x => x is not null)
            .Subscribe(x =>
            {
                if (x.GetType() == typeof(CollectionViewModel))
                {
                    RequestService.SelectedRequest = null;
                    RequestService.SelectedCollection = x as CollectionViewModel;
                }
                else if (x.GetType() == typeof(RequestViewModel))
                {
                    RequestService.SelectedCollection = null;
                    RequestService.SelectedRequest = x as RequestViewModel;
                }
                else
                {
                    throw new UnhandledErrorException();
                }
            });


        Func<CollectionViewModel, bool> CollectionFilter(string? text) => collection =>
        {
            return string.IsNullOrEmpty(text) ||
                   collection.Name!.ToLower().Contains(text.ToLower()) ||
                   (collection.Requests != null &&
                    collection.Requests.Any(r => r.Name!.ToLower().Contains(text.ToLower())));
        };

        var filterPredicate = this.WhenAnyValue(x => x.SearchCollectionText)
            .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
            .DistinctUntilChanged()
            .Select(CollectionFilter);

        _cleanUp = _sourceList.Connect()
            .RefCount()
            .Filter(filterPredicate)
            .Bind(out _collection)
            .DisposeMany()
            .Subscribe();
    }

    private ReactiveObject? _selectedNode;

    public ReactiveObject? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }


    private SourceList<CollectionViewModel> _sourceList = new();
    private readonly ReadOnlyObservableCollection<CollectionViewModel> _collection;
    public ReadOnlyObservableCollection<CollectionViewModel> Collections => _collection;


    private readonly IDisposable _cleanUp;

    public void Dispose()
    {
        _cleanUp.Dispose();
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

    private void ContextMenuDeleteCollection(CollectionViewModel collectionViewModel)
    {
        _sourceList.Remove(collectionViewModel);

        if (RequestService.SelectedCollection == collectionViewModel)
            RequestService.SelectedCollection = null;
    }

    public ICommand CtxCollectionPropertiesCommand { get; }

    private void ContextMenuSeeCollectionProperties(CollectionViewModel collectionViewModel)
    {
        SelectedNode = collectionViewModel;
    }

    private void CreateCollection()
    {
        _sourceList.Edit((update) =>
        {
            update.Add(new CollectionViewModel(new Collection
            {
                Name = "Unnamed Collection",
                Requests = new List<Request>()
            }));
        });
    }

    private string? _searchCollectionText;

    public string? SearchCollectionText
    {
        get => _searchCollectionText;
        set => this.RaiseAndSetIfChanged(ref _searchCollectionText, value);
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
                    new()
                    {
                        Action = EnumActions.DELETE, Name = "Delete Action", Endpoint = "http://localhost:5200/test/get"
                    },
                    new()
                    {
                        Action = EnumActions.OPTIONS, Name = "Options Aqui",
                        Endpoint = "http://localhost:5200/test/options"
                    },
                    new()
                    {
                        Action = EnumActions.POST, Name = "Post Json With something",
                        Endpoint = "http://localhost:5200/test/post"
                    },
                }
            }),
            new(new Collection
            {
                Name = "Col 2.",
                Requests = new List<Request>
                {
                    new()
                    {
                        Action = EnumActions.HEAD, Name = "PICK THE values", Endpoint = "http://localhost:5200/test2"
                    },
                    new() { Action = EnumActions.PUT, Name = "Placeholder", Endpoint = "http://localhost:5200/test2" },
                    new() { Action = EnumActions.PATCH, Name = "Patchhhhhh", Endpoint = "http://localhost:5200/test2" },
                }
            }),
        };
    }
}