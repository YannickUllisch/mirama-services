using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Infrastructure.Persistence;

namespace Mirama.Modules.Identity.Infrastructure.Services.Organizations;

internal sealed class OrganizationService(IdentityDbContext db) : IOrganizationService
{
    public async Task<OrganizationDto?> GetOrganizationByIdAsync(
        Guid organizationId, CancellationToken ct = default)
    {
        var org = await db.Organizations.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(organizationId), ct);

        return org is null ? null : Map(org);
    }

    public async Task<IReadOnlyList<OrganizationDto>> GetOrganizationsByTenantAsync(
        Guid tenantId, CancellationToken ct = default)
    {
        var orgs = await db.Organizations.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            .ToListAsync(ct);

        return orgs.Select(Map).ToList();
    }

    private static OrganizationDto Map(Organization o) => new(
        o.Id.Value,
        o.TenantId,
        o.Name,
        o.Slug,
        o.Logo,
        o.Street,
        o.City,
        o.Country,
        o.ZipCode,
        o.DateCreated);
}
