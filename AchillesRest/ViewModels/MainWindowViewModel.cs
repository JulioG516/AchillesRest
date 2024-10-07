using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using AchillesRest.Models;
using AchillesRest.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData.Binding;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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
        RequestService.SaveChanges();
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


        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.ShutdownRequested += async delegate(object? sender, ShutdownRequestedEventArgs e) 
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Unsaved changes",
                        "You have unsaved changes. Do you want to save before exiting?", ButtonEnum.YesNoCancel);

                var result = await box.ShowAsPopupAsync(lifetime.MainWindow);
                if (result == ButtonResult.Cancel)
                {
                    e.Cancel = true;
                }
                
            };  
        }

        Router.Navigate.Execute(ViewModelCollection[0]);
    }
}