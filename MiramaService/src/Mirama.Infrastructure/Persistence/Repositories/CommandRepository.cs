
using Mirama.Domain.Abstractions.Core;
using Mirama.Application.Common.Interfaces;

namespace Mirama.Infrastructure.Persistence.Repositories;

public sealed class CommandRepository<T, TID>(ApplicationDbContext dbContext) : ICommandRepository<T, TID> where T : AggregateRoot<TID>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public IQueryable<T> Query()
    {
        return _dbContext.Set<T>();
    }
}