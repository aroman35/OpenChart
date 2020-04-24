using System;
using Newtonsoft.Json;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Domain.Entities
{
    public class CandleDto : IDto
    {
        [JsonProperty("Id")] public Guid Id { get; set; }
        [JsonProperty("o")] public decimal Open { get; set; }
        [JsonProperty("c")] public decimal Close { get; set; }
        [JsonProperty("h")] public decimal High { get; set; }
        [JsonProperty("l")] public decimal Low { get; set; }
        [JsonProperty("v")] public decimal Volume { get; set; }
        [JsonProperty("date")] public long Date { get; set; }
    }
}