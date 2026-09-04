using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceCommandRepository<T, TID>(WorkspaceDbContext dbContext) : IWorkspaceCommandRepository<T, TID>
    where T : Entity<TID>
{
    public IQueryable<T> Query() => dbContext.Set<T>();
    public void Add(T entity) => dbContext.Set<T>().Add(entity);
    public void Update(T entity) => dbContext.Set<T>().Update(entity);
    public void Remove(T entity) => dbContext.Set<T>().Remove(entity);
}
