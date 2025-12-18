
namespace AccountService.Application.Domain.Abstractions.Tenant;

public interface ITenantScoped
{
    Guid OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}