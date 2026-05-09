using Mirama.Modules.Identity.Domain.Aggregates.Organization;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations;

internal static class OrganizationMapper
{
    internal static OrganizationResponse MapResponse(this Organization org) => new()
    {
        Id = org.Id.Value,
        Name = org.Name,
        Slug = org.Slug,
        Logo = org.Logo,
        Street = org.Street,
        City = org.City,
        Country = org.Country,
        ZipCode = org.ZipCode,
        DateCreated = org.DateCreated,
        TenantId = org.TenantId
    };
}
