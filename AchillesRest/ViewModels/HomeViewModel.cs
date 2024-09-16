using ReactiveUI;

namespace AchillesRest.ViewModels;

public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "HomeViewModel";

    public IScreen HostScreen { get; }
    
    public HomeViewModel() { }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
    
    public ViewModelBase MenuCollectionVM { get; } = new MenuCollectionsViewModel();

}