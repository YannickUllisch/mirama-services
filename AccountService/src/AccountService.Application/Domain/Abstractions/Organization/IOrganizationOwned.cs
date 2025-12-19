
namespace AccountService.Application.Domain.Abstractions.Organization;

public interface IOrganizationOwned
{
    Guid OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}