using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class CollectionManagerViewModel : ViewModelBase
{
    public CollectionManagerViewModel()
    {
        Authentications =
            new ObservableCollection<EnumAuthTypes>(Enum.GetValues(typeof(EnumAuthTypes)).Cast<EnumAuthTypes>());

        SelectedAuthentication = new BasicAuthentication();


        RequestService = Locator.Current.GetService<RequestService>()!;

        // RequestService.WhenAnyValue(x => x.SelectedCollection)
        //     .Subscribe(x => { Debug.WriteLine($"Valor do RequestService collection: {x}"); });

        // For mantain the Authentication properly.
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

    public RequestService RequestService { get; }

    private IAuthentication? _selectedAuthentication;

    public IAuthentication? SelectedAuthentication
    {
        get => _selectedAuthentication;
        set => this.RaiseAndSetIfChanged(ref _selectedAuthentication, value);
    }

    public ObservableCollection<EnumAuthTypes> Authentications { get; }
}