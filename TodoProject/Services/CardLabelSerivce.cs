using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Runtime;
using TodoProject.models;
using TodoProject.Models;
using TodoProject.Models.DTOs;

namespace TodoProject.Services
{
    public class CardLabelSerivce
    {
        public IOptions<TodoDatabaseSettings> dbSettings { get; }
        private IMongoCollection<CardLabel> cardLabelCollection;

        public CardLabelSerivce(IOptions<TodoDatabaseSettings> dbSettings, AppDbService appDb)
        {
            this.dbSettings = dbSettings;
            cardLabelCollection = appDb.MongoDb.GetCollection<CardLabel>(dbSettings.Value.CardLabelCollectionName);
        }

        public async Task<List<Label>> GetListLabelByCard(string cardId)
        {
            BsonDocument pipelineStage1 = new BsonDocument
            {
                {
                    "$match", new BsonDocument {
                        { "card_id", ObjectId.Parse(cardId) }
                    }
                }
            };

            BsonDocument pipelineStage2 = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument
                    {
                        { "from", dbSettings.Value.LabelsCollectionName },
                        { "localField", "label_id" },
                        { "foreignField", "_id" },
                        { "as", "label" } // label: Array of label ([{}])
                    }
                }
            };

            // map array of label to label (label: [{}] => label: {})
            BsonDocument pipelineStage3 = new BsonDocument
            {
                { "$unwind", "$label" }
            };

            //BsonDocument pipelineStage3_1 = new BsonDocument
            //{
            //    {
            //        "$addFields", new BsonDocument {
            //            { "label.priority", "$priority" }
            //        }
            //    }
            //};

            //BsonDocument pipelineStage4 = new BsonDocument
            //{
            //    {
            //        "$group", new BsonDocument
            //        {
            //            { "_id", "$card_id" },
            //            { "labels", new BsonDocument {
            //                    { "$addToSet", "$label" }
            //                }
            //            }
            //        }
            //    }
            //};

            //BsonDocument pipelineStage5 = new BsonDocument
            //{
            //    {
            //        "$project", new BsonDocument
            //        {
            //            { "_id", 1 },
            //            { "labels", 1 },
            //        }
            //    }
            //};

            BsonDocument[] pipeLine = {
                pipelineStage1,
                pipelineStage2,
                pipelineStage3,
                //pipelineStage3_1,
                //pipelineStage4,
            };
            var bsonDoc = await cardLabelCollection.Aggregate<BsonDocument>(pipeLine).ToListAsync();
            //var cardLabel = BsonSerializer.Deserialize<CardLabel>(bsonDoc);
            List<Label> result = new List<Label>();

            foreach (var doc in bsonDoc)
            {
                var cardLabel = BsonSerializer.Deserialize<CardLabel>(doc);
                if (cardLabel?.Label != null)
                {
                    cardLabel.Label.CardPriority = cardLabel.Priority;
                    result.Add(cardLabel.Label);
                }
            }

            return result;
        }

        public async Task<Boolean> AddLabel(string id, CardAddLabelRequest body)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(body?.LabelId)) return false;
            var cardLabel = await cardLabelCollection.Find(x => x.CardId == id && x.LabelId == body.LabelId).FirstOrDefaultAsync();
            if (cardLabel != null) return false;
            CardLabel _cardLabel = new CardLabel()
            {
                CardId = id,
                LabelId = body.LabelId,
                Priority = body.Priority > 0 ? body.Priority : 1,
            };
            await cardLabelCollection.InsertOneAsync(_cardLabel);
            return true;
        }

        public async Task<Boolean> RemoveLabel(string cardId, string labelId)
        {
            if (string.IsNullOrWhiteSpace(cardId) || string.IsNullOrWhiteSpace(labelId)) return false;
            var result = await cardLabelCollection.DeleteOneAsync(x => x.CardId == cardId && x.LabelId == labelId);
            return result.DeletedCount > 0;
        }
    }
}
