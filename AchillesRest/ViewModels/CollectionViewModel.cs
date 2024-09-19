using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AchillesRest.Models;
using AchillesRest.Models.Authentications;
using AchillesRest.Models.Enums;
using Avalonia.Media;
using ReactiveUI;

namespace AchillesRest.ViewModels;

// MVVM
public class CollectionViewModel : ViewModelBase
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private ObservableCollection<RequestViewModel>? _requests;

    public ObservableCollection<RequestViewModel>? Requests
    {
        get => _requests;
        set => this.RaiseAndSetIfChanged(ref _requests, value);
    }

    private IAuthentication? _authentication;

    public IAuthentication? Authentication
    {
        get => _authentication;
        set => this.RaiseAndSetIfChanged(ref _authentication, value);
    }

    private EnumAuthTypes? _selectedAuthType;

    public EnumAuthTypes? SelectedAuthType
    {
        get => _selectedAuthType;
        set => this.RaiseAndSetIfChanged(ref _selectedAuthType, value);
    }


    public CollectionViewModel(Collection collection)
    {
        Name = collection.Name;
        Requests = new ObservableCollection<RequestViewModel>(
            collection.Requests?.Select(r => new RequestViewModel(r)) ?? new List<RequestViewModel>()
        );
        SelectedAuthType = collection.SelectedAuthType;
        Authentication = collection.Authentication;
    }

    public override string ToString()
    {
        return $"Name:{Name}\nRequests Count:{Requests?.Count}\nAuthentication:{Authentication}";
    }
}