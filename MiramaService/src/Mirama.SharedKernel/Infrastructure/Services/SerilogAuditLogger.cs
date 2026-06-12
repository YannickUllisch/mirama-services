using Microsoft.Extensions.Logging;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.SharedKernel.Infrastructure.Services;

internal sealed class AuditLogEntry { }

internal sealed class SerilogAuditLogger(ILogger<AuditLogEntry> logger) : IAuditLogger
{
    public void LogRead(
        string operationName,
        string userId,
        string? tenantId,
        string? organizationId,
        string? projectId,
        string outcome,
        string traceId)
    {
        using (logger.BeginScope(new Dictionary<string, object?>
        {
            ["IsAuditLog"]     = true,
            ["AuditKind"]      = "Read",
            ["OperationName"]  = operationName,
            ["UserId"]         = userId,
            ["TenantId"]       = tenantId,
            ["OrganizationId"] = organizationId,
            ["ProjectId"]      = projectId,
            ["Outcome"]        = outcome,
            ["TraceId"]        = traceId,
        }))
        {
            logger.LogInformation(
                "[Audit] Read {OperationName} {Outcome} by {UserId}",
                operationName, outcome, userId);
        }
    }

    public void LogWrite(
        string entityType,
        string entityId,
        string operation,
        string userId,
        string? tenantId,
        string? organizationId,
        string? projectId,
        IReadOnlyList<AuditPropertyChange> changes,
        string traceId)
    {
        using (logger.BeginScope(new Dictionary<string, object?>
        {
            ["IsAuditLog"]     = true,
            ["AuditKind"]      = "Write",
            ["EntityType"]     = entityType,
            ["EntityId"]       = entityId,
            ["Operation"]      = operation,
            ["UserId"]         = userId,
            ["TenantId"]       = tenantId,
            ["OrganizationId"] = organizationId,
            ["ProjectId"]      = projectId,
            ["Changes"]        = changes,
            ["TraceId"]        = traceId,
        }))
        {
            logger.LogInformation(
                "[Audit] Write {Operation} {EntityType} {EntityId} ({ChangeCount} properties) by {UserId}",
                operation, entityType, entityId, changes.Count, userId);
        }
    }
}
