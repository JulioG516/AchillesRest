using AchillesRest.ViewModels;
using ReactiveUI;

namespace AchillesRest.Services;

public class RequestService : ReactiveObject
{
    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set => this.RaiseAndSetIfChanged(ref _selectedRequest, value);
    }
}