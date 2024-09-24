using AchillesRest.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class CollectionManagerView : ReactiveUserControl<CollectionManagerViewModel>
{
    public CollectionManagerView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}