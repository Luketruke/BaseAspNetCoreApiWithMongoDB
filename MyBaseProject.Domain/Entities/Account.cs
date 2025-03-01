using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyBaseProject.Domain.Entities
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("first_name")]
        public string? FirstName { get; set; }

        [BsonElement("last_name")]
        public string? LastName { get; set; }

        [BsonElement("phone_number")]
        public string? PhoneNumber { get; set; }
    }
}
