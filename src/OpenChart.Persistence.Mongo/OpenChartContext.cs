﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OpenChart.Application.Common;
using OpenChart.Domain.Entities.Candles;
using OpenChart.Persistence.Serializers;

namespace OpenChart.Persistence
{
    public class OpenChartContext : IDbContext
    {
        private readonly IMongoClient _mongoClientM;
        private readonly IMongoClient _mongoClientH;
        private readonly IMongoClient _mongoClientD;
        private readonly IMongoDatabase _databaseM;
        private readonly IMongoDatabase _databaseH;
        private readonly IMongoDatabase _databaseD;
        public OpenChartContext(IOptions<MongoSettings> mongoSettings)
        {
            var _mongoUrlM = new MongoUrl(mongoSettings.Value.ConnectionStringM);
            var _mongoUrlH = new MongoUrl(mongoSettings.Value.ConnectionStringH);
            var _mongoUrlD = new MongoUrl(mongoSettings.Value.ConnectionStringD);

            _mongoClientM = new MongoClient(_mongoUrlM);
            _mongoClientH = new MongoClient(_mongoUrlH);
            _mongoClientD = new MongoClient(_mongoUrlD);

            _databaseM = _mongoClientM.GetDatabase(_mongoUrlM.DatabaseName);
            _databaseH = _mongoClientH.GetDatabase(_mongoUrlH.DatabaseName);
            _databaseD = _mongoClientD.GetDatabase(_mongoUrlD.DatabaseName);

            InitClassMap();
        }

        private IMongoDatabase DataBase(DataBaseType dbType) => (dbType) switch
        {
            DataBaseType.Min => _databaseM,
            DataBaseType.Hour => _databaseH,
            DataBaseType.Day => _databaseD,
            _ => throw new MongoException("Unable to specify database")
        };

        private void InitClassMap()
        {
            BsonSerializer.RegisterSerializer(typeof(Candle), new CandleSerializer());
            BsonClassMap.RegisterClassMap<Candle>(cm =>
            {
                cm.MapMember(x => x.Date).SetElementName("t");
                cm.MapMember(x => x.Open).SetElementName("o");
                cm.MapMember(x => x.Close).SetElementName("c");
                cm.MapMember(x => x.High).SetElementName("h");
                cm.MapMember(x => x.Low).SetElementName("l");
                cm.MapMember(x => x.Volume).SetElementName("v");
            });
        }

        public int GetCollectionLength(string classCode, string securityCode, DataBaseType dbType)
        {
            return DataBase(dbType).RunCommand<dynamic>($"{{collstats: '{GetCollectionName(classCode, securityCode)}'}}").count;
        }

        public IMongoCollection<Candle> GetCollection(string classCode, string securityCode, DataBaseType dbType)
        {
            return DataBase(dbType).GetCollection<Candle>(GetCollectionName(classCode, securityCode));
        }

        public IMongoCollection<Candle> GetCollection(string collectionName, DataBaseType dbType)
        {
            return DataBase(dbType).GetCollection<Candle>(collectionName);
        }

        public async Task<List<string>> GetCollectionsList(DataBaseType dbType)
        {
            using var collectionsCursor = await DataBase(dbType).ListCollectionsAsync();
            var collections = await collectionsCursor.ToListAsync();
            return collections.Select(x => x.ToString()).ToList();
        }

        public async Task SetIndexes(DataBaseType dbType)
        {
            var allCollections = await GetCollectionsList(dbType);

            foreach (var collectionName in allCollections)
            {
                await GetCollection(collectionName, dbType)
                    .Indexes
                    .CreateOneAsync(
                        new CreateIndexModel<Candle>(
                            new IndexKeysDefinitionBuilder<Candle>().Ascending(x => x.Date)));
            }
        }

        public async Task SetIndex(string classCode, string securityCode, DataBaseType dbType)
        {
            await GetCollection(GetCollectionName(classCode, securityCode), dbType)
                .Indexes
                .CreateOneAsync(new CreateIndexModel<Candle>(
                    new IndexKeysDefinitionBuilder<Candle>().Ascending(x => x.Date)));
        }

        private string GetCollectionName(string classCode, string securityCode)
        {
            return $"{classCode}@{securityCode}";
        }
    }
}