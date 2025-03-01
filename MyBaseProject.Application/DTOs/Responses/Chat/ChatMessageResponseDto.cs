namespace MyBaseProject.Application.DTOs.Chat
{
    public class ChatMessageResponseDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime SentAt { get; set; }
    }
}
