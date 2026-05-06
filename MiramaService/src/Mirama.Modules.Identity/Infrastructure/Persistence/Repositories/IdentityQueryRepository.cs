using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;

public sealed class IdentityQueryRepository<T, TID>(IdentityDbContext dbContext) : IIdentityQueryRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly IdentityDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }
}