using System;
using System.Collections.ObjectModel;
using System.Linq;
using AchillesRest.Models.Enums;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class RequestManagerViewModel : ViewModelBase
{
    private EnumActions _selectedAction;

    public EnumActions SelectedAction
    {
        get => _selectedAction;
        set => this.RaiseAndSetIfChanged(ref _selectedAction, value);
    }

    public ObservableCollection<EnumActions> Actions { get; set; }

    public RequestManagerViewModel()
    {
        Actions = new ObservableCollection<EnumActions>(Enum.GetValues(typeof(EnumActions)).Cast<EnumActions>());
        SelectedAction = EnumActions.GET;

        RequestService = Locator.Current.GetService<RequestService>()!;
    }


    public RequestService RequestService { get; }
}