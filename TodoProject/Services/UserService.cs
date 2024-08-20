using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoProject.models;

namespace TodoProject.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> usersCollection;
        public UserService(IOptions<TodoDatabaseSettings> dbSettings, AppDbService appDb)
        {
            usersCollection = appDb.MongoDb.GetCollection<User>(dbSettings.Value.UsersCollectionName);
        }

        public async Task<User?> Login(Models.DTOs.LoginRequest body)
        {
            if (string.IsNullOrWhiteSpace(body?.username) || string.IsNullOrWhiteSpace(body?.password)) return null;
            var filter = Builders<User>.Filter.Eq(x => x.Username, body.username) &
                        Builders<User>.Filter.Eq(x => x.Password, body.password);
            
            return await usersCollection.Find(filter).FirstOrDefaultAsync();
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
    }
}
