using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OpenChart.Domain.Entities;

namespace OpenChart.Application.Common
{
    public interface IDbContext
    {
        IMongoCollection<CandleDto> GetCollection(string classCode, string securityCode);
        IMongoCollection<CandleDto> GetCollection(string collectionName);
        int GetCollectionLength(string classCode, string securityCode);
        Task<List<string>> GetCollectionsList();
        Task SetIndex(string classCode, string securityCode);
        Task SetIndexes();
    }
}