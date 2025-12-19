
using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Domain.Abstractions.Organization;

public abstract class OrganizationEntity<TID> : Entity<TID>, IOrganizationOwned
{
    public Guid OrganizationId { get; private set; } = default!;

    public Guid TenantId { get; private set; } = default!;

    void IOrganizationOwned.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = organizationId;
    }
}