namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when input fails a domain-level validation rule that cannot be expressed
/// as a simple guard (e.g. composite rules across multiple fields).
/// Prefer ErrorOr validation at aggregate factory boundaries; use this inside
/// constructors or property setters where a return type is unavailable.
/// </summary>
public sealed class DomainValidationException : DomainException
{
    public string Field { get; }

    public DomainValidationException(string field, string reason)
        : base($"Validation failed for '{field}': {reason}")
    {
        Field = field;
    }
}
