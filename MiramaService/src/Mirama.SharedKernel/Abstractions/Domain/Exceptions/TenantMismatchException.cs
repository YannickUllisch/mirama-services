namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when an operation targets an entity that belongs to a different tenant.
/// Acts as a second-level guard in domain logic behind the auth layer.
/// </summary>
public sealed class TenantMismatchException : DomainException
{
    public Guid ExpectedTenantId { get; }
    public Guid ActualTenantId { get; }

    public TenantMismatchException(Guid expectedTenantId, Guid actualTenantId)
        : base($"Tenant mismatch: expected '{expectedTenantId}', got '{actualTenantId}'.")
    {
        ExpectedTenantId = expectedTenantId;
        ActualTenantId = actualTenantId;
    }
}
