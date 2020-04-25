using System;
using OpenChart.Domain.Entities;

namespace OpenChart.Application.Entities
{
    public class InstrumentInfo : IInstrumentInfo
    {
        public InstrumentInfo(ITradeInstrument instrument, TimeSpan tradeDateStartOffset)
        {
            Instrument = instrument;
            TradeDateStartOffset = tradeDateStartOffset;
        }

        public ITradeInstrument Instrument { get; }
        public TimeSpan TradeDateStartOffset { get; }
        public TimeSpan TimeFrame { get; set; }
    }
}