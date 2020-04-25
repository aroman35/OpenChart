using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OpenChart.Application.Common;
using OpenChart.Domain.Entities;
using OpenChart.Persistence.Serializers;

namespace OpenChart.Persistence
{
    public class OpenChartContext : IDbContext
    {
        public OpenChartContext(string connectionString)
        {
            var mongoUrl = new MongoUrl(connectionString);
            IMongoClient mongoClient = new MongoClient(mongoUrl);
            DataBase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            InitClassMap();
        }

        public IMongoDatabase DataBase { get; }

        private void InitClassMap()
        {
            BsonSerializer.RegisterSerializer(typeof(CandleDto), new CandleSerializer());

            BsonClassMap.RegisterClassMap<CandleDto>(cm => { cm.AutoMap(); });
        }

        public int GetCollectionLength(string classCode, string securityCode)
        {
            return DataBase.RunCommand<dynamic>($"{{collstats: '{GetCollectionName(classCode, securityCode)}'}}").count;
        }

        public IMongoCollection<CandleDto> GetCollection(string classCode, string securityCode)
        {
            return DataBase.GetCollection<CandleDto>(GetCollectionName(classCode, securityCode));
        }

        public IMongoCollection<CandleDto> GetCollection(string collectionName)
        {
            return DataBase.GetCollection<CandleDto>(collectionName);
        }

        public async Task<List<string>> GetCollectionsList()
        {
            return (await (await DataBase.ListCollectionsAsync()).ToListAsync<BsonDocument>()).Select(x => x.ToString()).ToList();
        }

        public async Task SetIndexes()
        {
            var allCollections = await GetCollectionsList();

            foreach (var collectionName in allCollections)
            {
                await GetCollection(collectionName)
                    .Indexes
                    .CreateOneAsync(
                        new CreateIndexModel<CandleDto>(
                            new IndexKeysDefinitionBuilder<CandleDto>().Ascending(x => x.Date)));
            }
        }

        public async Task SetIndex(string classCode, string securityCode)
        {
            await GetCollection(GetCollectionName(classCode, securityCode))
                .Indexes
                .CreateOneAsync(new CreateIndexModel<CandleDto>(
                    new IndexKeysDefinitionBuilder<CandleDto>().Ascending(x => x.Date)));
        }

        private string GetCollectionName(string classCode, string securityCode)
        {
            return $"{classCode}@{securityCode}";
        }
    }
}