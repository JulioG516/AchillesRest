using AchillesRest.Services;
using Splat;

namespace AchillesRest.ViewModels;

public class ResponseViewModel : ViewModelBase
{
    public ResponseViewModel()
    {
        RequestService = Locator.Current.GetService<RequestService>()!;
    }

    public RequestService RequestService { get; }
}