using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Application.Interfaces.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> GetOrCreateChatAsync(string user1, string user2);
        Task AddMessageToChatAsync(string chatId, ChatMessage message);
        Task<List<Chat>> GetUserChatsAsync(string userId);
        Task<Chat> GetChatByIdAsync(string chatId);
    }
}
