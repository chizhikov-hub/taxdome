using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using TaxDome.ShadUI.Common.Localization;

namespace ShadUI.Demo;

public class App : Application
{
    // ReSharper disable once NotAccessedField.Local
    private static Mutex? _appMutex;

    public override void Initialize()
    {
        LocalizationManager.SetCulture("ru-RU");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        _appMutex = new Mutex(true, "ShadUISingleInstanceMutex", out var createdNew);
        if (!createdNew)
        {
            var instanceDialog = new InstanceDialog();
            instanceDialog.Show();
            return;
        }

        DisableAvaloniaDataAnnotationValidation();
        var provider = new ServiceProvider().RegisterDialogs();

        var themeWatcher = provider.GetService<ThemeWatcher>();
        themeWatcher.Initialize();
        var viewModel = provider.GetService<MainWindowViewModel>();
        viewModel.Initialize();

        var mainWindow = new MainWindow { DataContext = viewModel };
        this.RegisterTrayIconsEvents(mainWindow, viewModel);

        desktop.MainWindow = mainWindow;
        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
    }
}