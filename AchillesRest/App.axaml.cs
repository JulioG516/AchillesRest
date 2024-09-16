using AchillesRest.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AchillesRest.ViewModels;
using AchillesRest.Views;
using Splat;

namespace AchillesRest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        Locator.CurrentMutable.RegisterConstant(new RequestService(), typeof(RequestService));
        Locator.CurrentMutable.Register(() => new MenuCollectionsViewModel(), typeof(MenuCollectionsViewModel));
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