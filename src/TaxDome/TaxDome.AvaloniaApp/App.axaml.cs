using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TaxDome.AvaloniaApp.Localization;

namespace TaxDome.AvaloniaApp;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        LocalizationManager.SetCulture("ru-RU");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Views.DocumentHistoryView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}