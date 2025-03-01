using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using MyBaseProject.Infrastructure.Extensions.Settings;

namespace MyBaseProject.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Authentication:Jwt").Get<JwtSettings>();
        var googleSettings = configuration.GetSection("Authentication:Google").Get<GoogleSettings>();
        var facebookSettings = configuration.GetSection("Authentication:Facebook").Get<FacebookSettings>();

        if (string.IsNullOrEmpty(jwtSettings.SecretKey))
            throw new Exception("JWT SecretKey is missing in appsettings.json");

        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Unauthorized",
                        message = "You must provide a valid Bearer token in the Authorization header."
                    });
                }
            };
        })
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = googleSettings.ClientId;
            googleOptions.ClientSecret = googleSettings.ClientSecret;
        })
        .AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = facebookSettings.AppId;
            facebookOptions.AppSecret = facebookSettings.AppSecret;
        });

        return services;
    }
}
