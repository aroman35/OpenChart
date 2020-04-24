using System.Collections.Generic;
using System.Linq;
using OpenChart.Ddd.Aggregates;
using OpenChart.Ddd.Infrastructure;
using OpenChart.Domain.Exceptions;

namespace OpenChart.Domain.DddImplementation
{
    public class DomainTransaction<TAggregate> : ITransaction<TAggregate> where TAggregate : IAggregateRoot
    {
        private bool _isClosed;
        public LinkedList<TAggregate> OperationsList { get; }

        public void UpdateState(TAggregate aggregate)
        {
            if (IsClosed)
                throw new TransactionException<TAggregate>(OperationsList, "Transaction is closed");
            try
            {
                OperationsList.AddLast(aggregate);
            }
            catch
            {
                throw new TransactionException<TAggregate>(OperationsList);
            }
        }

        public TAggregate CurrentState()
        {
            if (!OperationsList.Any())
                throw new TransactionException<TAggregate>(OperationsList, "Transaction is empty");

            return OperationsList.Last.Value;
        }

        public TAggregate SourceState()
        {
            if (!OperationsList.Any())
                throw new TransactionException<TAggregate>(OperationsList, "Transaction is empty");

            return OperationsList.First.Value;
        }

        public void AbortTransaction()
        {
            _isClosed = true;
        }

        public bool IsClosed => _isClosed;

        public DomainTransaction()
        {
            OperationsList = new LinkedList<TAggregate>();
            _isClosed = false;
        }
    }
}