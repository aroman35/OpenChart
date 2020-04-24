using System;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Domain.DddImplementation
{
    public abstract class AggregateRoot<TDto> : IAggregateRoot<TDto> where TDto: IDto
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset CreationTime { get; protected set; }
        public DateTimeOffset LastUpdate { get; protected set; }

        public abstract int CompareTo(IAggregateRoot<TDto> other);

        public abstract bool Equals(IAggregateRoot<TDto> other);
    }
}