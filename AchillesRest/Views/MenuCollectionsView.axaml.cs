using AchillesRest.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class MenuCollectionsView : ReactiveUserControl<MenuCollectionsViewModel>
{
    public MenuCollectionsView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}