using MyBaseProject.Application.DTOs.Chat;
using MyBaseProject.Application.DTOs.Responses;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Application.Mappers
{
    public static class ChatMapper
    {
        public static ChatMessage ToChatMessage(this ChatMessageRequestDto request)
        {
            return new ChatMessage
            {
                SenderId = request.SenderId,
                Text = request.Text,
                SentAt = DateTime.UtcNow
            };
        }

        public static ChatMessageResponseDto ToChatMessageResponseDto(this ChatMessage message, string receiverId)
        {
            return new ChatMessageResponseDto
            {
                SenderId = message.SenderId,
                ReceiverId = receiverId,
                Text = message.Text,
                ImageUrl = message.ImageUrl,
                SentAt = message.SentAt
            };
        }
        public static async Task<ChatResponseDto> ToChatResponseDto(this Chat chat, IAccountRepository accountRepository, string currentUserId)
        {
            var otherUserId = chat.User1Id == currentUserId ? chat.User2Id : chat.User1Id;
            var otherUser = await accountRepository.GetByIdAsync(otherUserId);

            return new ChatResponseDto
            {
                ChatId = chat.Id,
                User = otherUser != null ? new AccountResponseDto
                {
                    AccountId = otherUser.Id.ToString(),
                    FirstName = otherUser.FirstName,
                    LastName = otherUser.LastName,
                    Email = otherUser.Email ?? string.Empty,
                    PhoneNumber = otherUser.PhoneNumber,
                } : new AccountResponseDto { AccountId = otherUserId },
                LastMessage = chat.Messages.Count > 0 ? chat.Messages[^1].Text : "No messages",
                LastUpdated = chat.LastUpdated
            };
        }

        public static async Task<ChatDetailResponseDto> ToChatDetailResponseDto(this Chat chat, IAccountRepository accountRepository, string currentUserId)
        {
            var otherUserId = chat.User1Id == currentUserId ? chat.User2Id : chat.User1Id;
            var otherUser = await accountRepository.GetByIdAsync(otherUserId);

            return new ChatDetailResponseDto
            {
                ChatId = chat.Id,
                User = otherUser != null ? new AccountResponseDto
                {
                    AccountId = otherUser.Id.ToString(),
                    FirstName = otherUser.FirstName,
                    LastName = otherUser.LastName,
                    Email = otherUser.Email ?? string.Empty,
                    PhoneNumber = otherUser.PhoneNumber,
                } : new AccountResponseDto { AccountId = otherUserId },
                Messages = chat.Messages.Select(msg => new ChatMessageResponseDto
                {
                    SenderId = msg.SenderId,
                    ReceiverId = otherUserId,
                    Text = msg.Text,
                    ImageUrl = msg.ImageUrl,
                    SentAt = msg.SentAt
                }).ToList()
            };
        }
    }
}
