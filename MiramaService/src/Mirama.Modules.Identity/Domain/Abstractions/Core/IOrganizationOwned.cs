
using Mirama.Domain.Aggregates.Organization;

namespace Mirama.Domain.Abstractions.Core;

public interface IOrganizationOwned
{
    OrganizationId OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}