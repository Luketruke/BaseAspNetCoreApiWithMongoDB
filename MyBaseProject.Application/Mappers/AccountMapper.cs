using MyBaseProject.Application.DTOs.Requests;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Application.Mappers
{
    public static class AccountMapper
    {
        public static AccountResponseDto MapToAccountResponseDto(Account account)
        {
            return new AccountResponseDto
            {
                AccountId = account.Id.ToString(),
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email ?? string.Empty,
                PhoneNumber = account.PhoneNumber,
            };
        }

        public static Account MapToAccount(AccountCreateRequestDto request, string hashedPassword)
        {
            return new Account
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = hashedPassword
            };
        }
        public static void UpdateAccountFromDto(AccountUpdateRequestDto request, Account existingAccount, string? hashedPassword = null)
        {
            existingAccount.FirstName = request.FirstName ?? existingAccount.FirstName;
            existingAccount.LastName = request.LastName ?? existingAccount.LastName;
            existingAccount.PhoneNumber = request.PhoneNumber ?? existingAccount.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Password) && hashedPassword != null)
            {
                existingAccount.Password = hashedPassword;
            }
        }
    }
}
