using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "HomeViewModel";

    public IScreen HostScreen { get; }

    public HomeViewModel() { }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        RequestService = Locator.Current.GetService<RequestService>()!;

        // RequestService.WhenAnyValue(x => x.SelectedRequest)
        //     .Where(x => x is not null)
        //     .ToProperty(this, x => x.SelRequest, out selRequest);
        
    }
    
    public ViewModelBase MenuCollectionVM { get; } = new MenuCollectionsViewModel();
    public RequestService RequestService { get; }

    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    }
}