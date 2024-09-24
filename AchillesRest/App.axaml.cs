using System;
using System.IO;
using AchillesRest.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AchillesRest.ViewModels;
using AchillesRest.Views;
using Avalonia.Platform;
using Splat;

namespace AchillesRest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        Locator.CurrentMutable.RegisterConstant(new RequestService(), typeof(RequestService));
        Locator.CurrentMutable.Register(() => new MenuCollectionsViewModel(), typeof(MenuCollectionsViewModel));


        // Loads an custom Json syntax highlight at AvaloniaEdit.
        var uri = new Uri("avares://AchillesRest/Assets/Json.xshd");
        using (var stream = AssetLoader.Open(uri))
        {
            using (var reader = new System.Xml.XmlTextReader(stream))
            {
                AvaloniaEdit.Highlighting.HighlightingManager.Instance.RegisterHighlighting("jsonachilles",
                    new string[0],
                    AvaloniaEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                        AvaloniaEdit.Highlighting.HighlightingManager.Instance));
            }
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}