using AchillesRest.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class RequestDetailsView : ReactiveUserControl<RequestDetailsViewModel>
{
    private readonly TextEditor _textEditor;

    public RequestDetailsView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
        _textEditor = this.FindControl<TextEditor>("textCode")!;
        _textEditor.TextArea.TextView.LinkTextForegroundBrush = Brushes.Cyan;
        // _textEditor.ShowLineNumbers = true;
        // _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
    }
}