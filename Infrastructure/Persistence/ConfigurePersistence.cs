using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new MongoDbService(config);
        });

        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetService<UserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetService<UserRepository>());
        
        services.AddScoped<SectionRepository>();
        services.AddScoped<ISectionRepository>(provider => provider.GetService<SectionRepository>());
        services.AddScoped<ISectionQueries>(provider => provider.GetService<SectionRepository>());
        
        services.AddScoped<SectionItemRepository>();
        services.AddScoped<ISectionItemRepository>(provider => provider.GetService<SectionItemRepository>());
        services.AddScoped<ISectionItemQueries>(provider => provider.GetService<SectionItemRepository>());
    }
}