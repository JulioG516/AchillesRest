using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                if (x!.GetType() == typeof(CollectionViewModel))
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

            if (RequestService.SelectedRequest == request)
                RequestService.SelectedRequest = null;
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
                Name = "Api Col 1",
                Description =
                    "# API Collection  - Example API\n\n## Description\nLorem ipsum odor amet, consectetuer adipiscing elit. Mauris aptent fames blandit potenti metus per turpis. Dictumst tortor accumsan cursus gravida laoreet ipsum sit. Rhoncus orci vestibulum varius eleifend; adipiscing taciti imperdiet sit. Justo cubilia lectus duis facilisis sagittis netus. Scelerisque congue senectus hac sed imperdiet hendrerit.\n\nFringilla natoque at mus semper ullamcorper amet taciti. Nec ligula donec euismod leo nisi accumsan. Inceptos facilisi cubilia scelerisque phasellus dapibus? Mus mattis fermentum donec accumsan ornare molestie eget vel. Proin porttitor praesent faucibus id laoreet rhoncus semper? Lacus hac diam sodales nisl mollis morbi nunc. Ut felis congue vitae inceptos, bibendum erat at mi. Amet suscipit aptent ridiculus amet taciti molestie porttitor.\n\nErat mollis eros magna curabitur dapibus hac integer senectus. Libero interdum libero integer bibendum ipsum phasellus curae. Massa lobortis cubilia mi nisl posuere suscipit. Augue fames ut natoque feugiat faucibus hac auctor nostra. Aliquam consequat posuere eu tempus nullam quisque. Taciti imperdiet magna vehicula vulputate massa curae congue rhoncus.\n\nDignissim nec conubia viverra; lectus feugiat ad. Eleifend dapibus eget fusce ridiculus viverra gravida. Feugiat magna fames id laoreet odio. Torquent vel blandit nam parturient a natoque lorem. Finibus integer integer dis aliquet sapien. Elementum mus dictumst facilisis lectus eu scelerisque vel magna mi. Suspendisse neque penatibus nascetur facilisi ex. Iaculis a dictumst rutrum penatibus eget efficitur sit.\n\nOrci dolor leo accumsan, ac curabitur tempus. Habitasse torquent nulla tellus leo torquent; tempus at sociosqu. Lacinia risus risus semper netus ad; proin id. Platea lacus interdum imperdiet felis commodo turpis aliquam. Sed felis phasellus maximus orci iaculis maecenas. Finibus ligula ultricies volutpat sed in taciti. Parturient primis lacus urna auctor laoreet. Ante ridiculus orci egestas in molestie eros ornare.\n\n",
                Requests = new List<Request>
                {
                    new()
                    {
                        Action = EnumActions.GET, Name = "Get Todos",
                        Endpoint = "https://jsonplaceholder.typicode.com/todos/1"
                    },
                    new()
                    {
                        Action = EnumActions.GET, Name = "Get Jobs",
                        Endpoint = "https://api.placeholderjson.dev/job-listings"
                    },
                    new()
                    {
                        Action = EnumActions.GET, Name = "Boarding Passes",
                        Endpoint = "https://api.placeholderjson.dev/boarding-pass"
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
                Name = "Api Col 2",
                Description =
                    "# API Collection 2 \n\n## Example API\n\n\n\n## Description\n\nLorem ipsum odor amet, consectetuer adipiscing elit. Enim efficitur magna justo pharetra etiam placerat vel odio. Penatibus dapibus nascetur taciti parturient, pellentesque scelerisque metus. Penatibus id hac dolor hac class risus; id potenti et. Potenti luctus erat nullam semper netus porttitor sed integer suspendisse. Dis libero at convallis tempus dis efficitur ipsum. Purus ad eros blandit; nec interdum mollis. Felis lorem pharetra placerat faucibus consequat risus donec neque nibh. Sit dignissim inceptos tellus vivamus gravida dictumst consectetur ultricies nisi.\n\nLeo netus iaculis id ac lacinia morbi. Erat velit sem libero inceptos volutpat neque. Etiam ipsum eros rhoncus, habitant tortor porttitor fusce. Placerat morbi luctus augue imperdiet diam mattis class. Egestas fusce magnis ridiculus in et venenatis. Dolor semper magna porta aptent integer fusce ut. Fermentum proin ad maximus, sit scelerisque duis. Elit posuere eleifend ultrices aliquam ad ultricies molestie placerat ante.",
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