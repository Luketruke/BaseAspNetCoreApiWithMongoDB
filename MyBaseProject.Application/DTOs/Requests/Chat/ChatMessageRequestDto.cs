namespace MyBaseProject.Application.DTOs.Chat
{
    public class ChatMessageRequestDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
    }
}
