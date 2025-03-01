using MyBaseProject.Application.Interfaces.Repositories;

namespace MyBaseProject.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IChatRepository Chats { get; }
    }
}
