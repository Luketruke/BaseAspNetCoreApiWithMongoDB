using MyBaseProject.Application.DTOs.Requests;
using MyBaseProject.Application.DTOs.Responses;

namespace MyBaseProject.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<List<AccountResponseDto>> GetAllAccountsAsync();
        Task<AccountResponseDto> GetAccountByIdAsync(string id);
        Task<bool> UpdateAccountAsync(string id, AccountUpdateRequestDto accountDto);
        Task<bool> DeleteAccountAsync(string id); 
        Task<AccountResponseDto> CreateAccountAsync(AccountCreateRequestDto accountDto);
    }
}
