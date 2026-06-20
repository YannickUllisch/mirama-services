using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Contracts.Organizations;

public interface IOrganizationService : IModuleService
{
    Task<OrganizationDto?> GetOrganizationByIdAsync(Guid organizationId, CancellationToken ct = default);
    Task<IReadOnlyList<OrganizationDto>> GetOrganizationsByTenantAsync(Guid tenantId, CancellationToken ct = default);
}
