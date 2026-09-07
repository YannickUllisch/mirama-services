
namespace Mirama.SharedKernel.Abstractions.Persistence;

public interface IAmbientMessageContext
{
    Guid? OrganizationId { get; }
    Guid? TenantId { get; }
    string? TraceId { get; }
    string? CorrelationId { get; }
    string? Headers { get; }

    /// <summary>Populates this scope's ambient context. Called once, before this scope's
    /// DbContext or handler is resolved.</summary>
    void Set(Guid? organizationId, Guid? tenantId, string? traceId, string? correlationId, string? headers);
}
