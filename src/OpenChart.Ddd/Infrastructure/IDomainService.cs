using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Ddd.Aggregates;

namespace OpenChart.Ddd.Infrastructure
{
    public interface IDomainService<TDomainEntity, TDto>
        where TDomainEntity: IAggregateRoot<TDto>
        where TDto: IDto
    {
        Task<TDomainEntity> Create(TDto dto, CancellationToken cancellationToken = default);
        Task<TDomainEntity> Create(IEnumerable<TDto> dto, CancellationToken cancellationToken = default);
        ITransaction<TDomainEntity> Transaction { get; }
    }
}