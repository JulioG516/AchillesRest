using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        RequestService = Locator.Current.GetService<RequestService>()!;

        Actions = new ObservableCollection<EnumActions>(Enum.GetValues(typeof(EnumActions)).Cast<EnumActions>());
        SelectedAction = EnumActions.GET;


        var canExecute = this.WhenAnyValue(x => x.RequestService.SelectedRequest!.Endpoint,
            (edp) => !string.IsNullOrEmpty(edp));
        SendRequestCommand = ReactiveCommand.CreateFromTask(SendRequest, canExecute);
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
        await RequestService.SendRequest();
    }


    public RequestService RequestService { get; }
}