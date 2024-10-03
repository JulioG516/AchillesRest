using System.Reactive;
using AchillesRest.Helpers;
using AchillesRest.ViewModels;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AchillesRest.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposable => { });
        AvaloniaXamlLoader.Load(this);

# if (DEBUG)
        {
            this.AttachDevTools();
        }
#endif
        Interactions.SetClipboard.RegisterHandler(async interaction =>
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var clipboard = desktop.MainWindow?.Clipboard;

                if (clipboard != null)
                    await clipboard.SetTextAsync(interaction.Input);
            }

            interaction.SetOutput(Unit.Default);
        });
    }
}