
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Infrastructure.Persistence.Repositories;

public sealed class ReadRepository<T, TID>(ApplicationDbContext dbContext) : IReadRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>().AsQueryable();
    }
}