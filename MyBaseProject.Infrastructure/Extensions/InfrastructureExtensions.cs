namespace MyBaseProject.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Infrastructure.Extensions.Settings;
using MyBaseProject.Infrastructure.Persistence;
using MyBaseProject.Infrastructure.Persistence.Repositories;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("Authentication:Jwt"));
        services.Configure<CorsSettings>(configuration.GetSection("Cors"));
        services.Configure<PusherSettings>(configuration.GetSection("Pusher"));
        services.Configure<GoogleSettings>(configuration.GetSection("Authentication:Google"));
        services.Configure<FacebookSettings>(configuration.GetSection("Authentication:Facebook"));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSecurity();
        services.AddExternalServices();

        services.AddSingleton<ILoggerFactory, LoggerFactory>();

        return services;
    }
}