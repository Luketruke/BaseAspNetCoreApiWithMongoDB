using System.Net.Mail;
using MyBaseProject.Application.DTOs.Requests;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.Services;
using MyBaseProject.Application.Mappers;
using MyBaseProject.Domain.Exceptions;

namespace MyBaseProject.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task<List<AccountResponseDto>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync();
            return accounts.Select(AccountMapper.MapToAccountResponseDto).ToList();
        }

        public async Task<AccountResponseDto> CreateAccountAsync(AccountCreateRequestDto request)
        {
            if (await _unitOfWork.Accounts.EmailExistsAsync(request.Email))
            {
                throw new EmailAlreadyExistsException(request.Email);
            }

            try
            {
                var mailAddress = new MailAddress(request.Email);
            }
            catch
            {
                throw new InvalidEmailFormatException();
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new RequiredPasswordException();
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var account = AccountMapper.MapToAccount(request, hashedPassword);
            await _unitOfWork.Accounts.CreateAsync(account);

            return AccountMapper.MapToAccountResponseDto(account);
        }
        public async Task<AccountResponseDto> GetAccountByIdAsync(string id)
        {
            Utils.Utils.ValidateObjectId(id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (account == null)
            {
                throw new EntityNotFoundException("account", id);
            }

            return AccountMapper.MapToAccountResponseDto(account);
        }
        public async Task<bool> UpdateAccountAsync(string id, AccountUpdateRequestDto accountDto)
        {
            Utils.Utils.ValidateObjectId(id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (account == null)
            {
                throw new EntityNotFoundException("account", id);
            }
            
            if (!string.IsNullOrWhiteSpace(accountDto.Password))
            {
                account.Password = _passwordHasher.HashPassword(accountDto.Password);
            }

            account.FirstName = accountDto.FirstName ?? account.FirstName;
            account.LastName = accountDto.LastName ?? account.LastName;
            account.Email = accountDto.Email ?? account.Email;

            await _unitOfWork.Accounts.UpdateAsync(id, account);
            return true;
        }

        public async Task<bool> DeleteAccountAsync(string id)
        {
            Utils.Utils.ValidateObjectId(id);

            await _unitOfWork.Accounts.DeleteAsync(id);

            var deletedAccount = await _unitOfWork.Accounts.GetByIdAsync(id);
            if (deletedAccount != null)
            {
                throw new InvalidOperationException($"Failed to delete the account with ID {id}.");
            }

            return true;
        }
    }
}
