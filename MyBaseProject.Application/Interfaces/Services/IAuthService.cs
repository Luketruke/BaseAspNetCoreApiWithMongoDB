using MyBaseProject.Application.DTOs.Responses;

namespace MyBaseProject.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AccountResponseDto> AuthenticateAsync(string email, string password);
        Task<AccountResponseDto> AuthenticateWithGoogleAsync(string idToken);
        Task<AccountResponseDto> AuthenticateWithFacebookAsync(string accessToken);
    }
}
