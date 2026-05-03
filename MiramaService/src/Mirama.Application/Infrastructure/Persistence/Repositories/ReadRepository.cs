
using Mirama.Application.Common.Interfaces;
using Mirama.Application.Domain.Abstractions.Core;

namespace Mirama.Application.Infrastructure.Persistence.Repositories;

public sealed class ReadRepository<T, TID>(ApplicationDbContext dbContext) : IReadRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>().AsQueryable();
    }
}