using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AchillesRest.Models;
using ReactiveUI;

namespace AchillesRest.ViewModels;

// MVVM
public class CollectionViewModel : ViewModelBase
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private ObservableCollection<RequestViewModel>? _requests;

    public ObservableCollection<RequestViewModel>? Requests
    {
        get => _requests;
        set => this.RaiseAndSetIfChanged(ref _requests, value);
    }


    
    public CollectionViewModel(Collection collection)
    {
        Name = collection.Name;
        Requests = new ObservableCollection<RequestViewModel>(
            collection.Requests?.Select(r => new RequestViewModel(r)) ?? new List<RequestViewModel>()
        );
    }
    
    
    
}