
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Organizations;

public sealed class OrganizationCommandRepository(ApplicationDbContext dbContext) : ICommandRepository<Organization>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task AddAsync(Organization entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Organizations.Add(entity);
        return Task.CompletedTask;
    }

   public async Task<ErrorOr<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var org = await _dbContext.Organizations.FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);
        if (org is null)
            return Error.NotFound("Organization not found.");

        _dbContext.Organizations.Remove(org);
        return Result.Success;
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Organizations
            .Include(o => o.Members)
            .Include(i => i.Invitations)
            .FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);
    }
}