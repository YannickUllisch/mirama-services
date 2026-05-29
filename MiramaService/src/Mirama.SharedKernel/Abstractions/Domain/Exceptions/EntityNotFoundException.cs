namespace Mirama.SharedKernel.Abstractions.Domain.Exceptions;

/// <summary>
/// Thrown when a required entity cannot be found by its identifier.
/// Use in domain operations that expect the entity to pre-exist.
/// </summary>
public sealed class EntityNotFoundException : DomainException
{
    public string EntityType { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityType, object entityId)
        : base($"{entityType} with id '{entityId}' was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public static EntityNotFoundException For<TEntity>(object id) =>
        new(typeof(TEntity).Name, id);
}
