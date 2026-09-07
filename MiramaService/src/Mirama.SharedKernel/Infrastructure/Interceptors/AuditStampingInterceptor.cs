using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Infrastructure.Interceptors;

/// <summary>
/// Stamps <see cref="IAuditable"/> created/modified fields and enforces tenant/organization
/// ownership (<see cref="ITenantOwned"/>, <see cref="IOrganizationOwned"/>) before every save.
/// </summary>
public sealed class AuditStampingInterceptor(IRequestContextProvider contextProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is not null)
            StampAndEnforceOwnership(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void StampAndEnforceOwnership(DbContext db)
    {
        string actorId;
        try { actorId = contextProvider.UserId.ToString(); }
        catch { actorId = "system"; }

        foreach (var entry in db.ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated(DateTime.UtcNow, actorId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, actorId);
                    break;
            }

            HandleEntityOwnership(entry);
        }
    }

    private void HandleEntityOwnership(EntityEntry<IAuditable> entry)
    {
        var tenantId = contextProvider.TenantId;
        var organizationId = contextProvider.OrganizationId;

        if (entry.Entity is ITenantOwned tenantOwned)
        {
            if (tenantId is null)
                throw new UnauthorizedAccessException($"TenantId is required to persist {entry.Entity.GetType().Name}");

            if (entry.State == EntityState.Added)
                tenantOwned.SetTenantId(tenantId.Value);
            else if (entry.State == EntityState.Modified && tenantOwned.TenantId != tenantId)
                throw new InvalidOperationException("Changing TenantId is not allowed");
        }

        if (entry.Entity is IOrganizationOwned orgOwned && entry.State == EntityState.Added)
        {
            if (orgOwned.OrganizationId == Guid.Empty)
            {
                if (organizationId is null)
                    throw new InvalidOperationException($"OrganizationId is required to persist {entry.Entity.GetType().Name}");
                orgOwned.SetOrganizationId(organizationId.Value);
            }
        }
    }
}
