using System;

namespace OpenChart.Domain.Entities
{
    public interface IInstrumentInfo
    {
        ITradeInstrument Instrument { get; }
        TimeSpan TradeDateStartOffset { get; }
        TimeSpan TimeFrame { get; set; }
    }
}