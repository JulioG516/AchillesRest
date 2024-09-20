using System;
using System.Reactive.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "HomeViewModel";

    public IScreen HostScreen { get; } = null!;

    public HomeViewModel()
    {
    }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        RequestService = Locator.Current.GetService<RequestService>()!;

        // RequestService.WhenAnyValue(x => x.SelectedRequest)
        //     .Where(x => x is not null)
        //     .ToProperty(this, x => x.SelRequest, out selRequest);

        // Request Manager VM - Configure the Response
        // RequestService.WhenAnyValue(x => x.ResponseMessage)
        //     .WhereNotNull()
        //     .Subscribe(OnNewResponse);


        // For mantain the Authentication properly. - Collection ViewModel
        RequestService.WhenAnyValue(x => x.SelectedCollection!.SelectedAuthType)
            .DistinctUntilChanged()
            .Subscribe(authType =>
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
                    default:
                        throw new ArgumentException("Invalid authentication type.");
                }

                if (RequestService.SelectedCollection != null)
                {
                    if (RequestService.SelectedCollection.Authentication?.GetType() != newAuth?.GetType())
                    {
                        RequestService.SelectedCollection.Authentication = newAuth;
                    }
                }
            });
    }

    public ViewModelBase MenuCollectionVm { get; } = new MenuCollectionsViewModel();
    public ViewModelBase RequestManagerVm { get; } = new RequestManagerViewModel();
    public ViewModelBase CollectionManagerVm { get; } = new CollectionManagerViewModel();
    public ViewModelBase ResponseViewVm { get; } = new ResponseViewModel();

    public ViewModelBase RequestDetailsVm { get; } = new RequestDetailsViewModel();

    public RequestService RequestService { get; } = null!;

    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    }
}