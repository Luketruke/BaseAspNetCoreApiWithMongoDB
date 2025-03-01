using MyBaseProject.Application.DTOs.Responses;

namespace MyBaseProject.Application.DTOs.Chat
{
    public class ChatResponseDto
    {
        public string ChatId { get; set; }
        public AccountResponseDto User { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
