using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoProject.models;

namespace TodoProject.Services
{
    public class AppDbService
    {
        public IMongoDatabase MongoDb { get; set; }
        public AppDbService(IOptions<TodoDatabaseSettings> dbSettings) 
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            MongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        }
    }
}
