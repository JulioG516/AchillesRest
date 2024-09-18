using AchillesRest.ViewModels;
using Microsoft.VisualBasic;
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

    private CollectionViewModel? _selectedCollection;

    public CollectionViewModel? SelectedCollection
    {
        get => _selectedCollection;
        set => this.RaiseAndSetIfChanged(ref _selectedCollection, value);
    }
}