using System.Text.Json.Serialization;
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
        Region = Enum.GetName(org.Region)!,
        RegionValue = (int)org.Region,
        PrimaryColor = org.PrimaryColor,
        AccentColor = org.AccentColor,
        DateCreated = org.DateCreated,
        TenantId = org.TenantId,

    };
}

public sealed record OrganizationResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("logo")]
    public string? Logo { get; init; }

    [JsonPropertyName("street")]
    public string Street { get; init; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; init; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    [JsonPropertyName("zipCode")]
    public string ZipCode { get; init; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; init; } = string.Empty;

    [JsonPropertyName("regionValue")]
    public int RegionValue { get; init; }

    [JsonPropertyName("primaryColor")]
    public string? PrimaryColor { get; init; }

    [JsonPropertyName("accentColor")]
    public string? AccentColor { get; init; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("memberCount")]
    public int MemberCount { get; init; }

    [JsonPropertyName("projectCount")]
    public int ProjectCount { get; init; }
}
