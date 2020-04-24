using System;
using System.Collections.Generic;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Domain.Exceptions
{
    public class TransactionException<TAggregate> : Exception
    where TAggregate : IAggregateRoot
    {
        public readonly Type AggregateType;
        public readonly ICollection<TAggregate> TransactionsList;
        
        public TransactionException(ICollection<TAggregate> transactionsList)
        : base($"An error was thrown in transaction {typeof(TAggregate).Name}")
        {
            AggregateType = typeof(TAggregate);
            TransactionsList = transactionsList;
        }

        public TransactionException(ICollection<TAggregate> transactionsList, string message)
        : base(message)
        {
            AggregateType = typeof(TAggregate);
            TransactionsList = transactionsList;
        }
    }
}