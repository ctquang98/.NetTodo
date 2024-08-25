using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoProject.Models;

namespace TodoProject.models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("role_ids")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId> RoleIds { get; set; } = new List<ObjectId>();

        [BsonElement("roles")]
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
