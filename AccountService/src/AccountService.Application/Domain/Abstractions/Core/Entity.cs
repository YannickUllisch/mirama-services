
namespace AccountService.Application.Domain.Abstractions.Core;

public abstract class Entity<TID> : IAuditable
{
    public TID Id { get; protected set; } = default!;
    public DateTime Created { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public string? LastModifiedBy { get; private set; }

    protected Entity() { }

    protected Entity(TID id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TID> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return EqualityComparer<TID>.Default.Equals(Id, other.Id);
    }

    void IAuditable.SetCreated(DateTime created, string? createdBy)
    {
        Created = created;
        CreatedBy = createdBy;
    }

    void IAuditable.SetModified(DateTime lastModified, string? lastModifiedBy)
    {
        LastModified = lastModified;
        LastModifiedBy = lastModifiedBy;
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;
}