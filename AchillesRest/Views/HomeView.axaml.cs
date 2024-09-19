using AchillesRest.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{

    private readonly TextEditor _textEditor;
    public HomeView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);

        _textEditor = this.FindControl<TextEditor>("textResponse");
    }
}