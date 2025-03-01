using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Application.DTOs.Responses
{
    public class AccountResponseDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
