using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using AchillesRest.Models;
using AchillesRest.Services;
using ReactiveUI;
using Splat;

namespace AchillesRest.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    private ObservableCollection<IRoutableViewModel> ViewModelCollection { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateHome { get; }

    public ICommand SaveCommand { get; }

    private void SaveCollections()
    {
        RequestService.SaveCollections();
    }

    public RequestService RequestService;

    public MainWindowViewModel()
    {
        SaveCommand = ReactiveCommand.Create(SaveCollections);

        RequestService = Locator.Current.GetService<RequestService>()!;


        ViewModelCollection = new ObservableCollection<IRoutableViewModel>()
        {
            new HomeViewModel(this),
        };

        NavigateHome = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(ViewModelCollection[0])
        );

        Router.Navigate.Execute(ViewModelCollection[0]);
    }
}