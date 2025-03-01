using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyBaseProject.Domain.Entities
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("user1_id")]
        public string User1Id { get; set; }

        [BsonElement("user2_id")]
        public string User2Id { get; set; }

        [BsonElement("messages")]
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        [BsonElement("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
