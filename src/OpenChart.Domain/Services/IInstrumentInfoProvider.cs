using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Services
{
    public interface IInstrumentInfoProvider
    {
        IInstrumentInfo InstrumentInfo { get; set; }
        bool IsInstrumentSet { get; }
    }
}