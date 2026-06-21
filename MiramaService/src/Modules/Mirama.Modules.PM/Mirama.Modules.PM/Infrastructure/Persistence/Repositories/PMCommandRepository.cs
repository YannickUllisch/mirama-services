
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Infrastructure.Persistence.Repositories;

public sealed class IdentityCommandRepository<T, TID>(PMDbContext dbContext) : IPMCommandRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly PMDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>();
    }
}