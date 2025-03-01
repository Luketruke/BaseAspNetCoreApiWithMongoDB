using System.Text.Json.Serialization;
using MyBaseProject.Domain.Enums;

namespace MyBaseProject.Application.DTOs.Requests.Login
{
    public class SocialLoginRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoginProvider Provider { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
