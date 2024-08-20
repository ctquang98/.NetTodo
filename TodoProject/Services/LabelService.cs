using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;
using TodoProject.models;
using TodoProject.Models;
using TodoProject.Models.DTOs;

namespace TodoProject.Services
{
    public class LabelService
    {
        private readonly IMongoCollection<Label> labelsCollection;

        public LabelService(AppDbService appDb, IOptions<TodoDatabaseSettings> dbSettings)
        {
            labelsCollection = appDb.MongoDb.GetCollection<Label>(dbSettings.Value.LabelsCollectionName);
        }

        public async Task<Label?> Create(UpdateLabelRequest body)
        {
            if (string.IsNullOrWhiteSpace(body.Name)) return null;
            Label label = new Label() { Name = body.Name };
            await labelsCollection.InsertOneAsync(label);
            return label;
        }

        public async Task<Label?> Update(string id, UpdateLabelRequest body)
        {
            if (string.IsNullOrWhiteSpace(id) || body == null) return null;
            var label = await labelsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (label == null) return null;
            if (body.Name.Length > 0) label.Name = body.Name;
            await labelsCollection.ReplaceOneAsync(x => x.Id == id, label);
            return label;
        }

        public async Task<Boolean> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            var result = await labelsCollection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<AppPagination<Label>> GetList(FilterParams _params)
        {
            if (_params == null) return null;
            var filter = Builders<Label>.Filter.Empty;
            var sorter = Builders<Label>.Sort.Ascending(x => x.Id);

            if (_params.FilterFieldName?.ToLower() == "name" && !string.IsNullOrWhiteSpace(_params.FilterFieldValue))
            {
                filter = Builders<Label>.Filter.Regex("name", new MongoDB.Bson.BsonRegularExpression(_params.FilterFieldValue, "i"));
            }

            if (_params.SortFieldName?.ToLower() == "id" && _params.SortAsc == false)
            {
                sorter = Builders<Label>.Sort.Descending(x => x.Id);
            }

            var findFluent = labelsCollection.Find(filter).Sort(sorter);
            return await AppPagination<Label>.HandlePagination(findFluent, _params.Page, _params.PageSize);
        }
    }
}
