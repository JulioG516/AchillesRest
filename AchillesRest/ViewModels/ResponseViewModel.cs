using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AchillesRest.Helpers;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class ResponseViewModel : ViewModelBase
{
    private readonly Dictionary<RequestViewModel, int?> _requests;

    public ResponseViewModel()
    {
        _requests = new Dictionary<RequestViewModel, int?>();

        RequestService = Locator.Current.GetService<RequestService>()!;
        ClipboardCopyCommand = ReactiveCommand.CreateFromTask(ClipboardCopy);

        this.WhenAnyValue(x => x.RequestService.SelectedRequest)
            .WhereNotNull().DistinctUntilChanged()
            .Subscribe(_ => { SelectedTab = _requests!.GetValueOrDefault(RequestService.SelectedRequest, 0); });
    }

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

    public ICommand ClipboardCopyCommand { get; }

    private async Task ClipboardCopy()
    {
        if (RequestService.Response?.Content == null)
            return;

        await Interactions.SetClipboard.Handle(RequestService.Response.Content);
    }

    public RequestService RequestService { get; }
}