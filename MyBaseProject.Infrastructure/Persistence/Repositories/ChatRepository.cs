using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : BaseRepository<Chat>, IChatRepository
    {
        public ChatRepository(IMongoCollection<Chat> chats, ILogger<ChatRepository> logger)
            : base(chats, logger) { }

        public async Task<Chat> GetOrCreateChatAsync(string user1, string user2)
        {
            var filter = Builders<Chat>.Filter.Or(
                Builders<Chat>.Filter.And(
                    Builders<Chat>.Filter.Eq(c => c.User1Id, user1),
                    Builders<Chat>.Filter.Eq(c => c.User2Id, user2)
                ),
                Builders<Chat>.Filter.And(
                    Builders<Chat>.Filter.Eq(c => c.User1Id, user2),
                    Builders<Chat>.Filter.Eq(c => c.User2Id, user1)
                )
            );

            var chat = await _collection.Find(filter).FirstOrDefaultAsync();

            if (chat == null)
            {
                chat = new Chat { User1Id = user1, User2Id = user2 };
                await _collection.InsertOneAsync(chat);
            }

            return chat;
        }

        public async Task AddMessageToChatAsync(string chatId, ChatMessage message)
        {
            var filter = Builders<Chat>.Filter.Eq(c => c.Id, chatId);
            var update = Builders<Chat>.Update
                .Push(c => c.Messages, message)
                .Set(c => c.LastUpdated, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<List<Chat>> GetUserChatsAsync(string userId)
        {
            var filter = Builders<Chat>.Filter.Or(
                Builders<Chat>.Filter.Eq(c => c.User1Id, userId),
                Builders<Chat>.Filter.Eq(c => c.User2Id, userId)
            );

            return await _collection.Find(filter).SortByDescending(c => c.LastUpdated).ToListAsync();
        }

        public async Task<Chat> GetChatByIdAsync(string chatId)
        {
            var filter = Builders<Chat>.Filter.Eq(c => c.Id, chatId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
