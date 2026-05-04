
namespace Mirama.SharedKernel.Abstractions.Domain.Core;

public interface IOrganizationOwned
{
    Guid OrganizationId { get; }

    void SetOrganizationId(Guid organizationId);
}