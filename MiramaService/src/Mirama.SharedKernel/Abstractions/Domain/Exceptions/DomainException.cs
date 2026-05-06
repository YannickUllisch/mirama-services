namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Base for all domain exceptions. Thrown when a domain rule or invariant is violated
/// at a level where ErrorOr is not the appropriate return mechanism (constructors, guards).
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }

    protected DomainException(string message, Exception inner) : base(message, inner) { }
}
