
using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Domain.Abstractions.Tenant;

public abstract class TenantEntity<TID> : Entity<TID>, ITenantScoped
{
    public Guid OrganizationId { get; private set; } = default!;

    void ITenantScoped.SetOrganizationId(Guid organizationId)
    {
        if (OrganizationId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        OrganizationId = organizationId;
    }
}