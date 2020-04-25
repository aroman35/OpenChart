using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Services
{
    public class InstrumentInfoProvider : IInstrumentInfoProvider
    {
        public IInstrumentInfo InstrumentInfo { get; set; }

        public bool IsInstrumentSet => !string.IsNullOrEmpty(InstrumentInfo.Instrument.ClassCode) &&
                                       !string.IsNullOrEmpty(InstrumentInfo.Instrument.SecurityCode);
    }
}