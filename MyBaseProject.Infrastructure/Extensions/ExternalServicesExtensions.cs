namespace MyBaseProject.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MyBaseProject.Application.Interfaces.ExternalServices;
    using MyBaseProject.Infrastructure.ExternalServices;

    public static class ExternalServicesExtensions
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton<IPusherService, PusherService>();
            return services;
        }
    }
}
