using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using AchillesRest.Models;
using AchillesRest.Models.Enums;
using AchillesRest.ViewModels;
using JsonFormatterPlus;
using ReactiveUI;

namespace AchillesRest.Services;

public class RequestService : ReactiveObject
{
    private Dictionary<RequestViewModel, AchillesHttpResponse?> _responses = new();

    private RequestViewModel? _selectedRequest;

    public RequestViewModel? SelectedRequest
    {
        get => _selectedRequest;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRequest, value);

            if (_selectedRequest != null && _responses.TryGetValue(_selectedRequest, out var responseContent))
            {
                Response!.Content = responseContent!.Content;
            }
            else
            {
                Response = new AchillesHttpResponse();
            }
        }
    }

    private CollectionViewModel? _selectedCollection;

    public CollectionViewModel? SelectedCollection
    {
        get => _selectedCollection;
        set => this.RaiseAndSetIfChanged(ref _selectedCollection, value);
    }

    private AchillesHttpResponse? _response;

    public AchillesHttpResponse? Response
    {
        get => _response;
        set => this.RaiseAndSetIfChanged(ref _response, value);
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


    public async Task SendRequest()
    {
        if (Response == null)
            Response = new AchillesHttpResponse();


        if (Response?.Content != null)
            Response.Content = null;

        var request = SelectedRequest;

        if (request == null)
            return;

        IsLoading = true;

        HttpClient httpClient = new HttpClient();

        HttpMethod method = request.Action switch
        {
            EnumActions.GET => HttpMethod.Get,
            EnumActions.POST => HttpMethod.Post,
            EnumActions.DELETE => HttpMethod.Delete,
            EnumActions.HEAD => HttpMethod.Head,
            EnumActions.PUT => HttpMethod.Put,
            EnumActions.PATCH => HttpMethod.Patch,
            EnumActions.OPTIONS => HttpMethod.Options,
            _ => throw new InvalidOperationException("Invalid Request Action.")
        };

        var requestMessage = new HttpRequestMessage(method, request.Endpoint!);

        try
        {
            var responseMessage = await httpClient.SendAsync(requestMessage);
            Response.HttpMessage = responseMessage;

            var response = await responseMessage.Content.ReadAsStringAsync();
            string formattedJson = JsonFormatter.Format(response);
            Response.Content = formattedJson;


            // var responseContent = await responseMessage.Content.ReadAsStringAsync();
            // ResponseContent = responseContent;

            // Store the response content in the dictionary
        }
        catch (HttpRequestException e)
        {
            Response.Content = e.Message;
        }
        catch (SocketException e)
        {
            Response.Content = e.Message;
        }
        finally
        {
            IsLoading = false;

            if (request != null)
            {
                if (string.IsNullOrEmpty(Response.Content))
                    throw new InvalidCastException();
                _responses[request] = Response;
            }
        }
    }

    public override string ToString()
    {
        return $"Colection: {SelectedCollection}\nRequest: {SelectedRequest}";
    }
}