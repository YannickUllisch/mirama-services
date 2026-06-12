using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags;

internal static class TagMapper
{
    internal static TagResponse MapResponse(this Tag tag) => new()
    {
        Id = tag.Id.Value,
        Name = tag.Name,
        Slug = tag.Slug,
        Color = tag.Color,
        Description = tag.Description,
        Scope = Enum.GetName(tag.Scope)!,
        ScopeValue = (int)tag.Scope,
        OrganizationId = tag.OrganizationId,
        DateCreated = tag.DateCreated
    };
}

public sealed record TagResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("scopeValue")]
    public int ScopeValue { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }
}
