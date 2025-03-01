using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Application.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account> CreateAsync(Account account); 
    }
}
