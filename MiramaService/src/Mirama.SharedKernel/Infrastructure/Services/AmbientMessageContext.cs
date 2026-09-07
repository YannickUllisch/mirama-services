
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.SharedKernel.Infrastructure.Services;

/// <summary>Default, scoped implementation of <see cref="IAmbientMessageContext"/>.
/// Unset (all null) for the lifetime of any scope nothing ever calls <see cref="Set"/>
/// on — a normal HTTP request scope, for instance, which sources context from
/// <c>HttpContext</c> instead and never touches this at all.</summary>
internal sealed class AmbientMessageContext : IAmbientMessageContext
{
    public Guid? OrganizationId { get; private set; }
    public Guid? TenantId { get; private set; }
    public string? TraceId { get; private set; }
    public string? CorrelationId { get; private set; }
    public string? Headers { get; private set; }

    public void Set(Guid? organizationId, Guid? tenantId, string? traceId, string? correlationId, string? headers)
    {
        OrganizationId = organizationId;
        TenantId = tenantId;
        TraceId = traceId;
        CorrelationId = correlationId;
        Headers = headers;
    }
}
