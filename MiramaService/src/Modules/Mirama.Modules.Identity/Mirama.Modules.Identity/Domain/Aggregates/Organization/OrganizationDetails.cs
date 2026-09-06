namespace Mirama.Modules.Identity.Domain.Aggregates.Organization;

public sealed record OrganizationDetails(
    string Name,
    string Slug,
    string Street,
    string City,
    string Country,
    string ZipCode,
    OrganizationRegion Region,
    string? Logo = null,
    string? PrimaryColor = null,
    string? AccentColor = null
);
