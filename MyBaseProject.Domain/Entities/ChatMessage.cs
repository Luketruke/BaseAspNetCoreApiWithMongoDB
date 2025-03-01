using MongoDB.Bson.Serialization.Attributes;

namespace MyBaseProject.Domain.Entities
{
    public class ChatMessage
    {
        [BsonElement("sender_id")]
        public string SenderId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("image_url")]
        public string? ImageUrl { get; set; }

        [BsonElement("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
