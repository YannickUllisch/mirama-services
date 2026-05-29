namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when an operation targets an entity that belongs to a different organization
/// than the one in the current request context.
/// </summary>
public sealed class OrganizationMismatchException : DomainException
{
    public Guid ExpectedOrganizationId { get; }
    public Guid ActualOrganizationId { get; }

    public OrganizationMismatchException(Guid expectedOrganizationId, Guid actualOrganizationId)
        : base($"Organization mismatch: expected '{expectedOrganizationId}', got '{actualOrganizationId}'.")
    {
        ExpectedOrganizationId = expectedOrganizationId;
        ActualOrganizationId = actualOrganizationId;
    }
}
