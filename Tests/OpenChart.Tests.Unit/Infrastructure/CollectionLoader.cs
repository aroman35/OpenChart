using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Entities.Candles;

namespace OpenChart.Tests.Unit.Infrastructure
{
    public class CollectionLoader
    {
        public async IAsyncEnumerable<Candle> LoadTestData(string classCode, string securityCode, TimeFrame timeFrame)
        {
            using var file = File.OpenText($"Infrastructure/{securityCode}@{classCode}_{timeFrame.ToString()}.json");
            var json = await file.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<Candle>>(json);
            foreach (var candle in data)
            {
                yield return candle;
            }
        }
    }
}