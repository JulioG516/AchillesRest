using AchillesRest.Services;
using Splat;

namespace AchillesRest.ViewModels;

public class CollectionManagerViewModel : ViewModelBase
{
    public CollectionManagerViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;
    }
    
    public RequestService RequestService { get; }
}