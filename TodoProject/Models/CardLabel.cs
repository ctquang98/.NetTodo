using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoProject.Models
{
    public class CardLabel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("card_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CardId { get; set; }

        [BsonElement("label_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LabelId { get; set; }

        [BsonElement("label")]
        public Label Label { get; set; }

        [BsonElement("labels")]
        public List<Label> Labels { get; set; }

        [BsonElement("priority")]
        public int Priority { get; set; }
    }
}
