using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace AchillesRest.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    private ObservableCollection<IRoutableViewModel> ViewModelCollection { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateHome { get; }

    
    public MainWindowViewModel()
    {
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