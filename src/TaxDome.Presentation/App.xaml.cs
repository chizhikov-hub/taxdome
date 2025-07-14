using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TaxDome.Presentation.Extensions;
using TaxDome.Presentation.Localization;
using TaxDome.Presentation.Views;

namespace TaxDome.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        // LocalizationManager.SetCulture("ru-RU");
        LocalizationManager.SetCulture("en-US");
    }
    
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services
            .AddDatabase()
            .AddRepositories()
            .AddApplicationServices()
            .AddViewModels()
            .AddViews();

        ServiceProvider = services.BuildServiceProvider();

        // Запуск основного окна
        var mainWindow = ServiceProvider.GetRequiredService<DocumentHistoryView>();
        mainWindow.Show();
    }
}