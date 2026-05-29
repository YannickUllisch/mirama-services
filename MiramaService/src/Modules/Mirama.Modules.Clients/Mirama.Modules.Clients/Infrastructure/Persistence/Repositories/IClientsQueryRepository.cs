using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;

public interface IClientsQueryRepository<TEntity, TId> where TEntity : Entity<TId>
{
    IQueryable<TEntity> Query();
}
