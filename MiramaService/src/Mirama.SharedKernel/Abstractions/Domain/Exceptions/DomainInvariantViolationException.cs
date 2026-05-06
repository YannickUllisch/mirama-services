namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when a domain invariant that must always hold is broken.
/// Use for intra-aggregate consistency rules that cannot be expressed as validation
/// (e.g. an aggregate that would leave itself in a logically impossible state).
/// </summary>
public sealed class DomainInvariantViolationException : DomainException
{
    public string Invariant { get; }

    public DomainInvariantViolationException(string invariant, string detail)
        : base($"Invariant '{invariant}' violated: {detail}")
    {
        Invariant = invariant;
    }
}
