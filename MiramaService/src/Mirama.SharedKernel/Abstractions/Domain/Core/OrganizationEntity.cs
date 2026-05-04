
namespace Mirama.SharedKernel.Abstractions.Domain.Core;

public abstract class OrganizationEntity<TID> : IAuditable, IOrganizationOwned
{
    public TID Id { get; protected set; } = default!;
    public DateTime Created { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public string? LastModifiedBy { get; private set; }

    public Guid OrganizationId { get; private set; } = default!;

    protected OrganizationEntity() { }

    protected OrganizationEntity(TID id) => Id = id;

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

    void IOrganizationOwned.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = organizationId;
    }
}