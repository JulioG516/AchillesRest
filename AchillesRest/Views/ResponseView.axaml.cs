using AchillesRest.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class ResponseView : ReactiveUserControl<ResponseViewModel>
{
    private readonly TextEditor _textEditor;

    public ResponseView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
        _textEditor = this.FindControl<TextEditor>("textResponse")!;
        _textEditor.TextArea.TextView.LinkTextForegroundBrush = Brushes.Cyan;
        // _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
    }
}