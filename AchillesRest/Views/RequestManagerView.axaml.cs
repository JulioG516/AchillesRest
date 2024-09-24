using AchillesRest.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class RequestManagerView : ReactiveUserControl<RequestManagerViewModel>
{
    public RequestManagerView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}