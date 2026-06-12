namespace Mirama.SharedKernel.Abstractions.Common.Interfaces;

public sealed record AuditPropertyChange(string Property, object? Before, object? After);

public interface IAuditLogger
{
    void LogRead(
        string operationName,
        string userId,
        string? tenantId,
        string? organizationId,
        string? projectId,
        string outcome,
        string traceId);

    void LogWrite(
        string entityType,
        string entityId,
        string operation,
        string userId,
        string? tenantId,
        string? organizationId,
        string? projectId,
        IReadOnlyList<AuditPropertyChange> changes,
        string traceId);
}
