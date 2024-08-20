using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TodoProject.models;
using TodoProject.Models;

namespace TodoProject.Services
{
    public class CommentService
    {
        private readonly IMongoCollection<Comment> commentCollection;
        private IOptions<TodoDatabaseSettings> dbSettings;
        private readonly CurrentUserAccessor currentUser;
        private readonly UserService userService;
        private readonly CardService cardService;

        public CommentService(
            IOptions<TodoDatabaseSettings> dbSettings,
            AppDbService appDb,
            CurrentUserAccessor currentUser,
            UserService userService,
            CardService cardService
        )
        {
            commentCollection = appDb.MongoDb.GetCollection<Comment>(dbSettings.Value.CommentCollectionName);
            this.dbSettings = dbSettings;
            this.currentUser = currentUser;
            this.userService = userService;
            this.cardService = cardService;
        }

        public async Task<Boolean> PostComment(string cardId, string content)
        {
            if (string.IsNullOrWhiteSpace(cardId) || string.IsNullOrWhiteSpace(content)) return false;
            string userId = currentUser.GetCurrentUserId();
            var user = await userService.GetById(userId);
            if (user == null) return false;

            Comment comment = new Comment()
            {
                Content = content,
                CardId = cardId,
                UserId = userId,
            };

            await commentCollection.InsertOneAsync(comment);
            return true;
        }

        public async Task<List<Comment>> GetListCommentByCardId(string cardId)
        {
            BsonDocument match = new BsonDocument
            {
                {
                    "$match", new BsonDocument {
                        { "card_id", ObjectId.Parse(cardId) }
                    }
                }
            };

            BsonDocument lookup = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument {
                        { "from", dbSettings.Value.UsersCollectionName },
                        { "localField", "user_id" },
                        { "foreignField", "_id" },
                        { "as", "user" },
                    }
                }
            };

            BsonDocument unwind = new BsonDocument
            {
                { "$unwind", "$user" }
            };

            BsonDocument project = new BsonDocument
            {
                {
                    "$project", new BsonDocument {
                        { "_id", 1 },
                        { "content", 1 },
                        { "user", 1 }
                    }
                }
            };

            BsonDocument[] pipline = { match, lookup, unwind, project };
            var bsonDoc = await commentCollection.Aggregate<BsonDocument>(pipline).ToListAsync();
            List<Comment> result = new List<Comment>();

            foreach (var doc in bsonDoc)
            {
                var comment = BsonSerializer.Deserialize<Comment>(doc);
                result.Add(comment);
            }

            return result;
        }
    }
}
