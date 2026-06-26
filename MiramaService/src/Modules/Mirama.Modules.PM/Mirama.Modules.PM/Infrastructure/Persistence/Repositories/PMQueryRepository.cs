using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Repositories;

internal sealed class PMQueryRepository<T, TID>(PMDbContext dbContext) : IPMQueryRepository<T, TID>
    where T : Entity<TID>
{
    public IQueryable<T> Query() => dbContext.Set<T>().AsNoTracking().AsQueryable();
}
