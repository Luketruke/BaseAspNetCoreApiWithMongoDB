using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Infrastructure.Extensions.Settings;

namespace MyBaseProject.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IMongoDatabase _database;
        private readonly ILoggerFactory _loggerFactory;

        public IAccountRepository Accounts { get; }
        public IChatRepository Chats { get; }

        public UnitOfWork(IMongoClient mongoClient, IOptions<DatabaseSettings> settings, ILoggerFactory loggerFactory,
                          IAccountRepository accountRepository, IChatRepository chatRepository)
        {
            _database = mongoClient.GetDatabase(settings.Value.Database);
            _loggerFactory = loggerFactory;
            Accounts = accountRepository;
            Chats = chatRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
