using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;

internal class ClientsCommandRepository<TEntity, TId>(ClientsDbContext db)
    : IClientsCommandRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    public void Add(TEntity entity) => db.Set<TEntity>().Add(entity);
    public void Update(TEntity entity) => db.Set<TEntity>().Update(entity);
    public void Remove(TEntity entity) => db.Set<TEntity>().Remove(entity);
}
