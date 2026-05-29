namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when an entity with the given identifier or unique key already exists.
/// Use to enforce uniqueness invariants inside aggregates.
/// </summary>
public sealed class EntityAlreadyExistsException : DomainException
{
    public string EntityType { get; }

    public EntityAlreadyExistsException(string entityType, string detail)
        : base($"{entityType} already exists: {detail}")
    {
        EntityType = entityType;
    }

    public static EntityAlreadyExistsException For<TEntity>(string detail) =>
        new(typeof(TEntity).Name, detail);
}
