using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaxDome.Application.Services;
using TaxDome.Domain.Repositories;
using TaxDome.Infrastructure;
using TaxDome.Infrastructure.Repositories;
using TaxDome.Presentation.Localization;
using TaxDome.Presentation.ViewModels;
using TaxDome.Presentation.Views;

namespace TaxDome.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public App()
    {
        LocalizationManager.SetCulture("ru-RU");
    }
    
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        ConfigureServices(services);

        ServiceProvider = services.BuildServiceProvider();

        // Запуск основного окна
        var mainWindow = ServiceProvider.GetRequiredService<DocumentHistoryView>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Настройка DbContext с использованием базы данных (пример с SQLite):
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));

        // Регистрация репозитория
        services.AddScoped<IDocumentRepository, DocumentRepository>(); 

        // Регистрация сервисов
        services.AddScoped<DocumentService>();

        // Регистрация ViewModel
        services.AddTransient<DocumentHistoryViewModel>();

        // Регистрация View
        services.AddTransient<DocumentHistoryView>();
    }
}