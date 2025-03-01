using MyBaseProject.Infrastructure.Extensions.Settings;

namespace MyBaseProject.API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CorsSettings>(configuration.GetSection("Cors"));

        var corsSettings = configuration.GetSection("Cors").Get<CorsSettings>();

        var allowedOrigins = corsSettings?.AllowedOrigins ?? new List<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            options.AddPolicy("AllowSpecificOrigins", builder =>
            {
                if (allowedOrigins.Any())
                {
                    builder.WithOrigins(allowedOrigins.ToArray())
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app, IConfiguration configuration)
    {
        var environment = configuration["Environment"];

        app.UseCors(environment == "Development" ? "AllowAll" : "AllowSpecificOrigins");

        return app;
    }
}
