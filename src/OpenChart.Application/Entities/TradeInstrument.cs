using OpenChart.Domain.Entities;

namespace OpenChart.Application.Entities
{
    public class TradeInstrument : ITradeInstrument
    {
        public TradeInstrument(string classCode, string securityCode)
        {
            ClassCode = classCode;
            SecurityCode = securityCode;
        }

        public string ClassCode { get; }
        public string SecurityCode { get; }
    }
}