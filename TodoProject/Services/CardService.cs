using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TodoProject.models;
using TodoProject.Models;
using TodoProject.Models.DTOs;

namespace TodoProject.Services
{
    public class CardService
    {
        public IOptions<TodoDatabaseSettings> dbSettings { get; }

        private readonly IMongoCollection<Card> cardsCollection;
        private CardLabelSerivce cardLabelSerivce;

        public CardService(IOptions<TodoDatabaseSettings> dbSettings, AppDbService appDb, CardLabelSerivce cardLabelSerivce)
        {
            this.dbSettings = dbSettings;
            this.cardLabelSerivce = cardLabelSerivce;
            cardsCollection = appDb.MongoDb.GetCollection<Card>(dbSettings.Value.CardsCollectionName);
        }

        public async Task<Card?> Create(CreateCardRequest body)
        {
            if (string.IsNullOrWhiteSpace(body.Name)) return null;
            Card card = new Card() { Name = body.Name, Description = body.Description };
            await cardsCollection.InsertOneAsync(card);
            return card;
        }

        public async Task<Card?> Update(string id, UpdateCardRequest body)
        {
            if (string.IsNullOrWhiteSpace(id) || body == null) return null;
            var card = await cardsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (card == null) return null;
            if (body.Name.Length > 0) card.Name = body.Name;
            if (body.Description.Length > 0) card.Description = body.Description;
            await cardsCollection.ReplaceOneAsync(x => x.Id == id, card);
            return card;
        }

        public async Task<Boolean> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var result = await cardsCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<Card> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return await cardsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AppPagination<Card>> GetList(FilterParams _params)
        {
            if (_params == null) return null;
            var filter = Builders<Card>.Filter.Empty;
            var sorter = Builders<Card>.Sort.Ascending(x => x.Id);

            if (_params.FilterFieldName?.ToLower() == "name" && !string.IsNullOrWhiteSpace(_params.FilterFieldValue))
            {
                filter = Builders<Card>.Filter.Regex("name", new MongoDB.Bson.BsonRegularExpression(_params.FilterFieldValue, "i"));
            }

            if (_params.SortFieldName?.ToLower() == "id" && _params.SortAsc == false)
            {
                sorter = Builders<Card>.Sort.Descending(x => x.Id);
            }

            var findFluent = cardsCollection.Find(filter).Sort(sorter);
            return await AppPagination<Card>.HandlePagination(findFluent, _params.Page, _params.PageSize);
        }
    }
}
