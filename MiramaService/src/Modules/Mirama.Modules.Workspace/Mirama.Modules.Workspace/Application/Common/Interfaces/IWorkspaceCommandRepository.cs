using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Workspace.Application.Common.Interfaces;

public interface IWorkspaceCommandRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
