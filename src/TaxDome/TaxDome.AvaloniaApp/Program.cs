using Avalonia;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaxDome.Application.Services;
using TaxDome.AvaloniaApp.Extensions;
using TaxDome.AvaloniaApp.Features.DocumentHistory;
using TaxDome.Domain.Repositories;
using TaxDome.Infrastructure;
using TaxDome.Infrastructure.Repositories;

namespace TaxDome.AvaloniaApp;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var services = ConfigureServices();
        BuildAvaloniaApp(services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider services)
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .AfterSetup(_ =>
            {
                App.Services = services;
            });
    
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services
            .AddDatabase()
            .AddRepositories()
            .AddApplicationServices()
            .AddViewModels()
            .AddViews();        

        return services.BuildServiceProvider();
    }
}
