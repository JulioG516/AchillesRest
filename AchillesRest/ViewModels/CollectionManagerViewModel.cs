using System;
using System.Collections.ObjectModel;
using System.Linq;
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