

using Mirama.Modules.Identity.Domain.Aggregates.Organization;

namespace Mirama.Modules.Identity.Domain.Abstractions.Core;

public interface IOrganizationOwned
{
    OrganizationId OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}