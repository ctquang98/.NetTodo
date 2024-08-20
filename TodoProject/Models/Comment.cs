using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoProject.models;

namespace TodoProject.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("card_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CardId { get; set; }

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("user")]
        public User user { get; set; }
    }
}
