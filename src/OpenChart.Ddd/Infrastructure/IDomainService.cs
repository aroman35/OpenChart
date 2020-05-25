using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Ddd.Infrastructure
{
    public interface IDomainService<TDomainEntity, TDto>
        where TDomainEntity: IAggregateRoot<TDto>
        where TDto: IDto
    {
        TDomainEntity Create(TDto dto);
        IEnumerable<TDomainEntity> Create(IEnumerable<TDto> dto);
        IAsyncEnumerable<TDomainEntity> Create(IAsyncEnumerable<TDto> dto, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}