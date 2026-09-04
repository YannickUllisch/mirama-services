using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Workspace.Application.Common.Interfaces;

public interface IWorkspaceQueryRepository<T, TID> where T : Entity<TID>
{
    IQueryable<T> Query();
}
