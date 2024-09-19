using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class RequestManagerViewModel : ViewModelBase
{
    public RequestManagerViewModel()
    {
        Actions = new ObservableCollection<EnumActions>(Enum.GetValues(typeof(EnumActions)).Cast<EnumActions>());
        SelectedAction = EnumActions.GET;
        SendRequestCommand = ReactiveCommand.CreateFromTask(SendRequest);


        RequestService = Locator.Current.GetService<RequestService>()!;
    }

    public ObservableCollection<EnumActions> Actions { get; set; }
    private EnumActions _selectedAction;

    public EnumActions SelectedAction
    {
        get => _selectedAction;
        set => this.RaiseAndSetIfChanged(ref _selectedAction, value);
    }

    public ICommand SendRequestCommand { get; }

    private async Task SendRequest()
    {
        if (RequestService.ResponseContent != null)
            RequestService.ResponseContent = null;

        RequestService.IsLoading = true;

        var request = RequestService.SelectedRequest;

        if (request == null)
            return;

        HttpClient httpClient = new HttpClient()
            { };

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

        //TODO Call in thread so does not block UI.
        try
        {
            var responseMessage = await httpClient.SendAsync(requestMessage);
            
            RequestService.ResponseMessage = responseMessage ;
        }
        catch (HttpRequestException e)
        {
            RequestService.ResponseContent = e.Message;
        }
        catch (SocketException e)
        {
            RequestService.ResponseContent = e.Message;
        }
        finally
        {
            RequestService.IsLoading = false;
        }
    }


    public RequestService RequestService { get; }
}