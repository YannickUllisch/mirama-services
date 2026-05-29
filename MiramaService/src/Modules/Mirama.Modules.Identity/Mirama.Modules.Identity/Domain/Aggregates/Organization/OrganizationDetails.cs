namespace Mirama.Modules.Identity.Domain.Aggregates.Organization;

public sealed record OrganizationDetails(
    string Name,
    string Street,
    string City,
    string Country,
    string ZipCode,
    string? Logo = null
);
