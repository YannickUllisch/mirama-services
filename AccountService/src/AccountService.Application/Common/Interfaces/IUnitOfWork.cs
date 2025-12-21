

namespace AccountService.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync();
}