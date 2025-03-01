using System.Net.Mail;
using System.Text.Json;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.Services;
using MyBaseProject.Application.Mappers;
using MyBaseProject.Domain.Entities;
using MyBaseProject.Domain.Exceptions;

namespace MyBaseProject.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task<AccountResponseDto> AuthenticateAsync(string email, string password)
        {
            try
            {
                var mailAddress = new MailAddress(email);
            }
            catch
            {
                throw new InvalidEmailFormatException();
            }

            var account = await _unitOfWork.Accounts.GetAccountByEmailAsync(email);
            if (account == null)
            {
                throw new UserNotFoundException(email);
            }

            if (!_passwordHasher.VerifyPassword(password, account.Password))
            {
                throw new InvalidCredentialsException();
            }

            return AccountMapper.MapToAccountResponseDto(account);
        }
        public async Task<AccountResponseDto> AuthenticateWithGoogleAsync(string idToken)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={idToken}");

            if (!response.IsSuccessStatusCode)
                throw new InvalidCredentialsException();

            var payload = JsonSerializer.Deserialize<GooglePayload>(await response.Content.ReadAsStringAsync());

            if (payload == null || string.IsNullOrEmpty(payload.Email))
                throw new InvalidCredentialsException();

            var account = await _unitOfWork.Accounts.GetAccountByEmailAsync(payload.Email);
            if (account == null)
            {
                account = new Account
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                };
                await _unitOfWork.Accounts.CreateAsync(account);
            }

            return AccountMapper.MapToAccountResponseDto(account);
        }

        public async Task<AccountResponseDto> AuthenticateWithFacebookAsync(string accessToken)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://graph.facebook.com/me?fields=id,name,email,picture&access_token={accessToken}");

            if (!response.IsSuccessStatusCode)
                throw new InvalidCredentialsException();

            var payload = JsonSerializer.Deserialize<FacebookPayload>(await response.Content.ReadAsStringAsync());

            if (payload == null || string.IsNullOrEmpty(payload.Email))
                throw new InvalidCredentialsException();

            var account = await _unitOfWork.Accounts.GetAccountByEmailAsync(payload.Email);
            if (account == null)
            {
                account = new Account
                {
                    Email = payload.Email,
                    FirstName = payload.Name,
                };
                await _unitOfWork.Accounts.CreateAsync(account);
            }

            return AccountMapper.MapToAccountResponseDto(account);
        }

    }
}
