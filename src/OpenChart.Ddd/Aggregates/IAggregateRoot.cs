using System;

namespace OpenChart.Ddd.Aggregates
{
    public interface IAggregateRoot<TDto> : IAggregateRoot, IComparable<IAggregateRoot<TDto>>, IEquatable<IAggregateRoot<TDto>>
    where TDto: IDto
    {
    }

    public interface IAggregateRoot : IRoot
    {
        Guid Id { get; }
        DateTimeOffset CreationTime { get; }
        DateTimeOffset LastUpdate { get; }
    }
}