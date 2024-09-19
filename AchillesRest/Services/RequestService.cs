using System.Net.Http;
using AchillesRest.Models.Enums;
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

    private HttpResponseMessage? _responseMessage;

    public HttpResponseMessage? ResponseMessage
    {
        get => _responseMessage;
        set => this.RaiseAndSetIfChanged(ref _responseMessage, value);
    }

    private string? _responseContent;

    public string? ResponseContent
    {
        get => _responseContent;
        set => this.RaiseAndSetIfChanged(ref _responseContent, value);
    }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public override string ToString()
    {
        return $"Colection: {SelectedCollection}\nRequest: {SelectedRequest}";
    }
}