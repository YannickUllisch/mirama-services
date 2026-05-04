using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Abstractions.Core;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Repositories;

public sealed class ReadRepository<T, TID>(ApplicationDbContext dbContext) : IReadRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }
}