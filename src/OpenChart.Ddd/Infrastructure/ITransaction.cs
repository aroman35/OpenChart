using System.Collections.Generic;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Ddd.Infrastructure
{
    public interface ITransaction<TAggregate> where TAggregate : IAggregateRoot
    {
        LinkedList<TAggregate> OperationsList { get; }
        void UpdateState(TAggregate aggregate);
        TAggregate CurrentState();
        TAggregate SourceState();
        void AbortTransaction();
        bool IsClosed { get; }
    }
}