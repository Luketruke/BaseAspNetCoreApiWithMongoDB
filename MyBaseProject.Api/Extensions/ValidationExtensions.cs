using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;

namespace MyBaseProject.API.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            e => e.Key,
                            e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                        );

                    return new BadRequestObjectResult(new { errors });
                };
            });

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            return services;
        }
    }
}
