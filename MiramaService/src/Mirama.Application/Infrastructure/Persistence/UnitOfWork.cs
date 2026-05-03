
using Mirama.Application.Common.Interfaces;

namespace Mirama.Application.Infrastructure.Persistence;

public sealed class UnitOfWork(ApplicationDbContext dbcontext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbcontext;

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}