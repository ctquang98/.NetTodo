using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoProject.Models
{
    public class Card
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("due_date")]
        public DateTime DueDate { get; set; }

        public List<Label> labels { get; set; } = new List<Label>();

        //public List<string> commentIds { get; set; } = new List<string>();
        public List<Comment> comments { get; set; } = new List<Comment>();
    }
}
