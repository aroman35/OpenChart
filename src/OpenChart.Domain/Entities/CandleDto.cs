using System;
using Newtonsoft.Json;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Domain.Entities
{
    public class CandleDto : IDto, ICandle
    {
        public CandleDto()
        {
            Id = Guid.NewGuid();
        }
        [JsonIgnore]
        public DateTimeOffset TradeDateTime => DateTimeOffset.FromUnixTimeMilliseconds(Date);
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("o")] public decimal Open { get; set; }
        [JsonProperty("c")] public decimal Close { get; set; }
        [JsonProperty("h")] public decimal High { get; set; }
        [JsonProperty("l")] public decimal Low { get; set; }
        [JsonProperty("v")] public decimal Volume { get; set; }
        [JsonProperty("t")] public long Date { get; set; }
        public int CompareTo(ICandle other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return TradeDateTime.CompareTo(other.TradeDateTime);
        }
    }
}