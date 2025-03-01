namespace MyBaseProject.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MyBaseProject.Application.Interfaces.Services;
    using MyBaseProject.Infrastructure.Security;

    public static class SecurityExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
