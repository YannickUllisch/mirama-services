
using AccountService.Application.Domain.Aggregates.Organization;

namespace AccountService.Application.Domain.Abstractions.Core;

public interface IOrganizationOwned
{
    OrganizationId OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}