using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;

public interface IClientsCommandRepository<TEntity, TId> where TEntity : Entity<TId>
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
