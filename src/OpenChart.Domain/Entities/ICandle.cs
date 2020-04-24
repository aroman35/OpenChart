using System;

namespace OpenChart.Domain.Entities
{
    public interface ICandle : IComparable<ICandle>
    {
        DateTime TradeDateTime { get; }
        decimal Open { get; }
        decimal Close { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Volume { get; }
    }
}