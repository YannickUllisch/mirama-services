
using Mirama.Application.Domain.Aggregates.Organization;

namespace Mirama.Application.Domain.Abstractions.Core;

public interface IOrganizationOwned
{
    OrganizationId OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}