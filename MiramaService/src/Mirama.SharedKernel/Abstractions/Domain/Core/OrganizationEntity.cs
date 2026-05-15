
namespace Mirama.SharedKernel.Abstractions.Domain.Core;

public abstract class OrganizationEntity<TID> : Entity<TID>, IAuditable, IOrganizationOwned
{
    public Guid OrganizationId { get; private set; } = default!;

    void IOrganizationOwned.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = organizationId;
    }
}