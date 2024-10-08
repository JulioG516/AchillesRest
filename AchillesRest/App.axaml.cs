using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AchillesRest.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AchillesRest.ViewModels;
using AchillesRest.Views;
using Avalonia.Platform;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Splat;

namespace AchillesRest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Locator.CurrentMutable.Register(() => new LiteDbService(), typeof(IDbService));
        Locator.CurrentMutable.RegisterConstant(new LiteDbService(), typeof(IDbService));
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
            desktop.ShutdownRequested += This_ShutdownRequested;

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    // Save the changes made upon the collections and requests.
    protected virtual void This_ShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        var requestService = Locator.Current.GetService<RequestService>()!;

        if (requestService.Collection.Any(c => c.HasModifications))
            requestService.SaveChanges();


        Debug.WriteLine($"APP SHUTDOWN! {nameof(This_ShutdownRequested)}");
        OnShutdownRequest(e);
    }

    protected virtual void OnShutdownRequest(ShutdownRequestedEventArgs e)
    {
        ShutdownRequest?.Invoke(this, e);
    }

    public event EventHandler<ShutdownRequestedEventArgs>? ShutdownRequest;
}