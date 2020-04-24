namespace OpenChart.Domain.Entities
{
    public interface ITradeInstrument
    {
        string ClassCode { get; }
        string SecurityCode { get; }
    }
}