
using Mirama.Modules.Identity.Infrastructure.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;

public sealed class IdentityCommandRepository<T, TID>(ApplicationDbContext dbContext) : IIdentityCommandRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>();
    }
}