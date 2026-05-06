namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when an operation is called on an entity that is in an incompatible state.
/// Examples: accepting an already-accepted invitation, modifying a deleted entity,
/// assigning a role to a member who already has that role.
/// </summary>
public sealed class InvalidDomainOperationException : DomainException
{
    public string EntityType { get; }
    public string Operation { get; }

    public InvalidDomainOperationException(string entityType, string operation, string reason)
        : base($"Cannot '{operation}' on {entityType}: {reason}")
    {
        EntityType = entityType;
        Operation = operation;
    }
}
