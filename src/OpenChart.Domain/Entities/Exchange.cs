using OpenChart.Ddd.Aggregates;

namespace OpenChart.Domain.Entities
{
    public class Exchange : IValueObject
    {
        public string ClassCode { get; set; }
        public long TradeMorningStart { get; set; }
        public long TradeMorningEnd { get; set; }
        public long TradeStart { get; set; }
        public long TradeEnd { get; set; }
        public long TradeEveningStart { get; set; }
        public long TradeEveningEnd { get; set; }
    }
}