using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoProject.Models
{
    public class Label
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("priority")]
        public int? CardPriority { get; set; }
    }
}
