namespace Mirama.Modules.Identity.Contracts.Organizations;

public sealed record OrganizationDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string Slug,
    string? Logo,
    string Street,
    string City,
    string Country,
    string ZipCode,
    DateTime DateCreated);
