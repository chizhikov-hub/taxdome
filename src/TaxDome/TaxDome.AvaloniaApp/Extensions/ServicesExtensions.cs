using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaxDome.Application.Services;
using TaxDome.AvaloniaApp.Features.DocumentHistory;
using TaxDome.Domain.Repositories;
using TaxDome.Infrastructure;
using TaxDome.Infrastructure.Repositories;

namespace TaxDome.AvaloniaApp.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));
            
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();
        services.AddScoped<IDocumentActionRepository, DocumentActionRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        // services.AddScoped<IDocumentRepository, DocumentRepositoryStub>();
        
        return services;
    }
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<DocumentService>();
        
        return services;
    }
    
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<DocumentHistoryViewModel>();
        
        return services;
    }
    
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<DocumentHistoryView>();
        
        return services;
    }
}