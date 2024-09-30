using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AchillesRest.Models;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;
using DynamicData;
using JsonFormatterPlus;
using ReactiveUI;

namespace AchillesRest.Services;

public class RequestService : ReactiveObject
{
    public RequestService()
    {
        SourceList.AddRange(FillCollection());

        // For mantain the Authentication properly. - Collection ViewModel
        this.WhenAnyValue(x => x.SelectedCollection!.SelectedAuthType)
            .DistinctUntilChanged()
            .Subscribe(authType =>
            {
                var newAuth = GetAuthenticationFromEnum(authType);

                if (SelectedCollection != null)
                {
                    if (SelectedCollection.Authentication?.GetType() != newAuth?.GetType())
                    {
                        SelectedCollection.Authentication = newAuth;

                        // Filter the requests with SelectedAuthType == EnumAuthTypes.InheritFromParent
                        if (SelectedCollection.Requests != null)
                        {
                            var reqs = SelectedCollection.Requests
                                .Where(r => r.SelectedAuthType == EnumAuthTypes.InheritFromParent)
                                .ToList();

                            // Update the Authentication property for each filtered request
                            foreach (var req in reqs)
                            {
                                req.Authentication = newAuth;
                            }
                        }
                    }
                }
            });

        //
        this.WhenAnyValue(x => x.SelectedRequest!.SelectedAuthType)
            .DistinctUntilChanged()
            .Subscribe(authType =>
            {
                var newAuth = GetAuthenticationFromEnum(authType);

                if (SelectedRequest != null)
                {
                    if (SelectedRequest.Authentication?.GetType() != newAuth?.GetType())
                    {
                        SelectedRequest.Authentication = newAuth;
                    }
                }
            });
    }

    private IAuthentication? GetAuthenticationFromEnum(EnumAuthTypes? authType)
    {
        IAuthentication? newAuth;
        switch (authType)
        {
            case null:
                newAuth = null;
                break;
            case EnumAuthTypes.None:
                newAuth = null;
                break;
            case EnumAuthTypes.Basic:
                newAuth = new BasicAuthentication();
                break;
            case EnumAuthTypes.Bearer:
                newAuth = new BearerAuthentication();
                break;
            case EnumAuthTypes.Digest:
                newAuth = new DigestAuthentication();
                break;
            case EnumAuthTypes.InheritFromParent:
                newAuth = Collections
                    .Where(c => c.Requests != null && c.Requests.Any(r => Equals(r, SelectedRequest)))
                    .Select(c => c.Authentication)
                    .FirstOrDefault();
                break;
            default:
                throw new ArgumentException("Invalid authentication type.");
        }

        return newAuth;
    }

    public SourceList<CollectionViewModel> SourceList = new();
    public ReadOnlyObservableCollection<CollectionViewModel> Collection = null!;
    public ReadOnlyObservableCollection<CollectionViewModel> Collections => Collection;

    public void CreateCollection()
    {
        SourceList.Edit((update) =>
        {
            update.Add(new CollectionViewModel(new Collection
            {
                Name = "Unnamed Collection",
                Requests = new List<Request>()
            }));
        });
    }

    public void DeleteCollection(CollectionViewModel collectionViewModel)
    {
        SourceList.Remove(collectionViewModel);

        if (SelectedCollection == collectionViewModel)
            SelectedCollection = null;
    }

    public void DeleteRequest(RequestViewModel request)
    {
        var collection = Collections.FirstOrDefault(w => w.Requests != null && w.Requests.Contains(request));

        if (collection != null)
        {
            // Remove the request from the collection
            collection.Requests!.Remove(request);

            if (Equals(SelectedRequest, request))
                SelectedRequest = null;
        }
    }

    private readonly Dictionary<RequestViewModel, AchillesHttpResponse?> _responses = new();

    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRequest, value);

            if (_selectedRequest != null && _responses.TryGetValue(_selectedRequest, out var responseContent))
            {
                Response = responseContent;
            }
            else
            {
                Response = new AchillesHttpResponse();
            }
        }
    }

    private CollectionViewModel? _selectedCollection;

    public CollectionViewModel? SelectedCollection
    {
        get => _selectedCollection;
        set => this.RaiseAndSetIfChanged(ref _selectedCollection, value);
    }

    private AchillesHttpResponse? _response;

    public AchillesHttpResponse? Response
    {
        get => _response;
        set => this.RaiseAndSetIfChanged(ref _response, value);
    }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public async Task SendRequest()
    {
        Response ??= new AchillesHttpResponse();


        if (Response?.Content != null)
            Response.Content = null;

        var request = SelectedRequest;

        if (request == null)
            return;

        IsLoading = true;

        HttpClient httpClient = new HttpClient();

        HttpMethod method = request.Action switch
        {
            EnumActions.GET => HttpMethod.Get,
            EnumActions.POST => HttpMethod.Post,
            EnumActions.DELETE => HttpMethod.Delete,
            EnumActions.HEAD => HttpMethod.Head,
            EnumActions.PUT => HttpMethod.Put,
            EnumActions.PATCH => HttpMethod.Patch,
            EnumActions.OPTIONS => HttpMethod.Options,
            _ => throw new InvalidOperationException("Invalid Request Action.")
        };

        var requestMessage = new HttpRequestMessage(method, request.Endpoint!);

        if (request.Authentication != null)
            request.Authentication.ApplyAuthentication(requestMessage);

        if ((request.Action == EnumActions.POST
             || request.Action == EnumActions.PUT
             || request.Action == EnumActions.PATCH)
            && !string.IsNullOrEmpty(request.Body))
        {
            // Add body
            var content = new StringContent(request.Body, System.Text.Encoding.UTF8,
                "application/json");

            requestMessage.Content = content;
        }

        try
        {
            var responseMessage = await httpClient.SendAsync(requestMessage);
            Response!.HttpMessage = responseMessage;

            var response = await responseMessage.Content.ReadAsStringAsync();
            string formattedJson = JsonFormatter.Format(response);
            Response!.Content = formattedJson;


            // var responseContent = await responseMessage.Content.ReadAsStringAsync();
            // ResponseContent = responseContent;

            // Store the response content in the dictionary
        }
        catch (HttpRequestException e)
        {
            Response!.Content = e.Message;
        }
        catch (SocketException e)
        {
            Response!.Content = e.Message;
        }
        finally
        {
            IsLoading = false;

            _responses[request] = Response;
        }
    }

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
                        Action = EnumActions.POST, Name = "Post DummyJson",
                        Endpoint = "https://dummyjson.com/auth/login",
                        Body = "{\n\t\"username\": \"michaelw\",\n\t\"password\": \"michaelwpass\"\n}",
                        Headers = new List<Header>
                            { new() { Key = "Content-Type", Value = "application/json", Enabled = true } }
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

    public override string ToString()
    {
        return $"Collection: {SelectedCollection}\nRequest: {SelectedRequest}";
    }

    public void AddHeader()
    {
        if (SelectedRequest == null)
            return;

        SelectedRequest.Headers.Add(new HeaderViewModel() { Enabled = true });
    }
}