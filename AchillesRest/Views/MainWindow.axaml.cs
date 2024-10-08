using System;
using System.Linq;
using System.Reactive;
using AchillesRest.Helpers;
using AchillesRest.ViewModels;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
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

        Interactions.GetFolderDialog.RegisterHandler(async interaction =>
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var options = new FolderPickerOpenOptions
                {
                    AllowMultiple = false,
                    Title = interaction.Input
                };

                if (desktop.MainWindow == null)
                    throw new NullReferenceException();

                var folder = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(options);
                interaction.SetOutput(folder.Any() ? folder.First().Path.LocalPath : null);
            }
        });

        Interactions.GetFileDialog.RegisterHandler(async interaction =>
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow == null)
                    throw new NullReferenceException();

                var options = new FilePickerOpenOptions()
                {
                    AllowMultiple = false,
                    SuggestedFileName = "Exported Collections",
                    Title = interaction.Input
                };

                var file = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(options);

                interaction.SetOutput(file.Any() ? file.First().Path.LocalPath : null);
            }
        });
    }
}