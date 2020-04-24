using System;

namespace OpenChart.Ddd.Aggregates
{
    public interface IDomainEvent : IRoot
    {
        Type EventSource { get; }
    }
}