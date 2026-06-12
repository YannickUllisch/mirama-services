using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Messaging.Inbox;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.SharedKernel.Infrastructure.Interceptors;

public sealed class AuditSaveChangesInterceptor(
    IAuditLogger auditLogger,
    IRequestContextProvider context)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is not null)
            CaptureAuditEntries(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void CaptureAuditEntries(DbContext db)
    {
        string userId;
        try { userId = context.UserId.ToString(); }
        catch (UnauthorizedAccessException) { userId = "anonymous"; }

        var traceId   = Activity.Current?.TraceId.ToString() ?? string.Empty;
        var tenantId  = context.TenantId?.ToString();
        var orgId     = context.OrganizationId?.ToString();
        var projectId = context.ProjectId?.ToString();

        foreach (var entry in db.ChangeTracker.Entries())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                continue;

            if (entry.Entity is OutboxMessage or InboxMessage)
                continue;

            var entityType = entry.Entity.GetType().Name;
            var entityId   = GetEntityId(entry);
            var operation  = entry.State.ToString();
            var changes    = GetChanges(entry);

            auditLogger.LogWrite(entityType, entityId, operation,
                userId, tenantId, orgId, projectId, changes, traceId);
        }
    }

    private static string GetEntityId(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null) return "unknown";
        var values = key.Properties.Select(p => entry.Property(p.Name).CurrentValue?.ToString() ?? "null");
        return string.Join(",", values);
    }

    private static IReadOnlyList<AuditPropertyChange> GetChanges(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry) =>
        entry.State switch
        {
            EntityState.Added =>
                entry.Properties
                     .Select(p => new AuditPropertyChange(p.Metadata.Name, null, p.CurrentValue))
                     .ToList(),

            EntityState.Modified =>
                entry.Properties
                     .Where(p => p.IsModified)
                     .Select(p => new AuditPropertyChange(p.Metadata.Name, p.OriginalValue, p.CurrentValue))
                     .ToList(),

            EntityState.Deleted =>
                entry.Properties
                     .Select(p => new AuditPropertyChange(p.Metadata.Name, p.OriginalValue, null))
                     .ToList(),

            _ => [],
        };
}
