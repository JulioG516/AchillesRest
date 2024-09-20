using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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

        this.WhenAnyValue(x => x.RequestService.SelectedRequest)
            .WhereNotNull()
            .DistinctUntilChanged()
            .Subscribe(_ => { SelectedTab = _requests!.GetValueOrDefault(RequestService.SelectedRequest, 0); });
    }

    public RequestService RequestService { get; }

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