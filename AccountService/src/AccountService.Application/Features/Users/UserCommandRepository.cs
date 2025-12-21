
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Users;

public sealed class UserCommandRepository(ApplicationDbContext dbContext) : ICommandRepository<User>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        _dbContext.User.Add(entity);
        return Task.CompletedTask;
    }

   public async Task<ErrorOr<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);
        if (user is null)
            return Error.NotFound("User not found.");

        _dbContext.User.Remove(user);
        return Result.Success;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.User.FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);
    }
}