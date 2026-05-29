using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;

internal class ClientsQueryRepository<TEntity, TId>(ClientsDbContext db)
    : IClientsQueryRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    public IQueryable<TEntity> Query() => db.Set<TEntity>();
}
