using System.Threading.Tasks;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Ddd.Infrastructure
{
    public interface IDomainEventBus
    {
        Task Send(IDomainEvent @event);
    }
}