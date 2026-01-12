
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Organizations;

public sealed class TenantCommandRepository(ApplicationDbContext dbContext) : ICommandRepository<Tenant>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task AddAsync(Tenant entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Tenants.Add(entity);
        return Task.CompletedTask;
    }

   public async Task<ErrorOr<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (tenant is null)
            return Error.NotFound("Organization not found.");

        _dbContext.Tenants.Remove(tenant);
        return Result.Success;
    }

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tenants
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}