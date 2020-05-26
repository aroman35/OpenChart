using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OpenChart.Domain.Entities.Candles;

namespace OpenChart.Application.Common
{
    public interface IDbContext
    {
        IMongoCollection<Candle> GetCollection(string classCode, string securityCode, DataBaseType dbType);
        IMongoCollection<Candle> GetCollection(string collectionName, DataBaseType dbType);
        int GetCollectionLength(string classCode, string securityCode, DataBaseType dbType);
        Task<List<string>> GetCollectionsList(DataBaseType dbType);
        Task SetIndex(string classCode, string securityCode, DataBaseType dbType);
        Task SetIndexes(DataBaseType dbType);
    }
}