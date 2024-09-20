using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class RequestDetailsViewModel : ViewModelBase
{
    private readonly Dictionary<RequestViewModel, int?> _requests;

    public RequestDetailsViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;
        _requests = new Dictionary<RequestViewModel, int?>();
        
        Authentications =
            new ObservableCollection<EnumAuthTypes>(Enum.GetValues(typeof(EnumAuthTypes)).Cast<EnumAuthTypes>());
        
        this.WhenAnyValue(x => x.RequestService.SelectedRequest)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(_ => { SelectedTab = _requests!.GetValueOrDefault(RequestService.SelectedRequest, 0); });
    }

    public RequestService RequestService { get; }

    public ObservableCollection<EnumAuthTypes> Authentications { get; }
    
    private int? _selectedTab;
    public int? SelectedTab
    {
        get => _selectedTab;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTab, value);

            _requests[RequestService.SelectedRequest!] = SelectedTab;
        }
    }
}