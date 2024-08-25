using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TodoProject.models;
using TodoProject.Models;
using TodoProject.Models.DTOs;

namespace TodoProject.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> usersCollection;
        private readonly IOptions<TodoDatabaseSettings> dbSettings;

        public UserService(IOptions<TodoDatabaseSettings> dbSettings, AppDbService appDb)
        {
            usersCollection = appDb.MongoDb.GetCollection<User>(dbSettings.Value.UsersCollectionName);
            this.dbSettings = dbSettings;
        }

        public async Task<User?> Login(Models.DTOs.LoginRequest body)
        {
            if (string.IsNullOrWhiteSpace(body?.username) || string.IsNullOrWhiteSpace(body?.password)) return null;
            //var filter = Builders<User>.Filter.Eq(x => x.Username, body.username) &
            //            Builders<User>.Filter.Eq(x => x.Password, body.password);

            //return await usersCollection.Find(filter).FirstOrDefaultAsync();

            var match = new BsonDocument
            {
                {
                    "$match", new BsonDocument {
                        { "username", body.username },
                        { "password", body.password },
                    }
                }
            };

            var lookup = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument {
                        { "from", dbSettings.Value.RolesCollectionName },
                        { "localField", "role_ids" },
                        { "foreignField", "_id" },
                        { "as", "roles" },
                    }
                }
            };

            BsonDocument[] pipline = { match, lookup };
            var bsonDoc = await usersCollection.Aggregate<BsonDocument>(pipline).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<User>(bsonDoc);
        }

        public async Task<User?> Register(Models.DTOs.RegisterRequest body)
        {
            if (string.IsNullOrWhiteSpace(body?.Username) || string.IsNullOrWhiteSpace(body?.Email) || string.IsNullOrWhiteSpace(body?.Password))
            {
                return null;
            }

            var filter = Builders<User>.Filter.Eq(x => x.Username, body.Username);
            var user = await usersCollection.Find(filter).FirstOrDefaultAsync();
            if (user != null) return null;

            User _user = new User()
            {
                Email = body.Email,
                Username = body.Username,
                Password = body.Password,
            };

            await usersCollection.InsertOneAsync(_user);
            return _user;
        }

        public async Task<List<User>> GetAll()
        {
            return await usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return await usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Boolean> Update(User _user)
        {
            if (string.IsNullOrWhiteSpace(_user?.Id)) return false;
            var x = await usersCollection.ReplaceOneAsync(x => x.Id == _user.Id, _user);
            return x.ModifiedCount > 0;
        }
    }
}
