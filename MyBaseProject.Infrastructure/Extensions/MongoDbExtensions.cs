namespace MyBaseProject.Infrastructure.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using MyBaseProject.Domain.Entities;
    using MyBaseProject.Infrastructure.Extensions.Settings;

    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IMongoClient>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(settings.Server);
            });

            services.AddScoped<IMongoDatabase>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                var client = provider.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.Database);
            });

            services.AddScoped<IMongoCollection<Account>>(provider =>
            {
                var database = provider.GetRequiredService<IMongoDatabase>();
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                return database.GetCollection<Account>(settings.AccountsCollection ?? throw new Exception("AccountsCollection is not set."));
            });

            services.AddScoped<IMongoCollection<Chat>>(provider =>
            {
                var database = provider.GetRequiredService<IMongoDatabase>();
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                return database.GetCollection<Chat>(settings.ChatsCollection ?? throw new Exception("ChatsCollection is not set."));
            });

            return services;
        }
    }
}