using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence.Repositories;

internal sealed class WorkspaceQueryRepository<T, TID>(WorkspaceDbContext dbContext) : IWorkspaceQueryRepository<T, TID>
    where T : Entity<TID>
{
    public IQueryable<T> Query() => dbContext.Set<T>().AsNoTracking().AsQueryable();
}
