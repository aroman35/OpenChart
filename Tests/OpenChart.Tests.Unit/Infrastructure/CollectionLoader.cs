using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OpenChart.Domain.Entities;

namespace OpenChart.Tests.Unit.Infrastructure
{
    public class CollectionLoader
    {
        public async IAsyncEnumerable<Candle> LoadTestData(string classCode, string securityCode)
        {
            using var file = File.OpenText($"Infrastructure/{securityCode}@{classCode}.json");
            var json = await file.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<CandleDto>>(json);
            foreach (var dto in data)
            {
                yield return new Candle(dto, classCode, securityCode);
            }
        }

        public async IAsyncEnumerable<CandleDto> LoadTestDataDto(string classCode, string securityCode)
        {
            using var file = File.OpenText($"Infrastructure/{securityCode}@{classCode}.json");
            var json = await file.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<CandleDto>>(json);
            foreach (var dto in data)
            {
                yield return dto;
            }
        }
    }
}