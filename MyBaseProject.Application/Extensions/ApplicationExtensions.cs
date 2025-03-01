using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.Services;
using MyBaseProject.Application.Services;
using MyBaseProject.Application.Validators;

namespace MyBaseProject.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountCreateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AccountUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        return services;
    }
}