using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace OpenChart.Domain.Entities.Candles
{
    public class Candle : IComparable<Candle>, IEquatable<Candle>
    {
        public Candle()
        {
            Id = ObjectId.GenerateNewId();
        }
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [JsonIgnore]
        public DateTimeOffset TradeDateTime => DateTimeOffset.FromUnixTimeMilliseconds(Date);
        [JsonProperty("o")] public decimal Open { get; set; }
        [JsonProperty("c")] public decimal Close { get; set; }
        [JsonProperty("h")] public decimal High { get; set; }
        [JsonProperty("l")] public decimal Low { get; set; }
        [JsonProperty("v")] public decimal Volume { get; set; }
        [JsonProperty("t")] public long Date { get; set; }

        public int CompareTo(Candle other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return TradeDateTime.CompareTo(other.TradeDateTime);
        }

        public bool Equals(Candle other)
        {
            return other != null && Date == other.Date;
        }
    }
}