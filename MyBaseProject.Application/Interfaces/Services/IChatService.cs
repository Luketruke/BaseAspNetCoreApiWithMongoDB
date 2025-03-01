using System.Threading.Tasks;
using MyBaseProject.Application.DTOs.Chat;

namespace MyBaseProject.Application.Interfaces
{
    public interface IChatService
    {
        Task<ChatMessageResponseDto> SendMessageAsync(ChatMessageRequestDto request);
        Task<List<ChatResponseDto>> GetUserChatsAsync(string userId);
        Task<ChatDetailResponseDto> GetChatByIdAsync(string chatId, string userId);
    }
}
