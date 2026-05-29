using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

internal static class TeamMapper
{
    internal static TeamResponse MapResponse(this Team team) => new()
    {
        Id = team.Id.Value,
        Name = team.Name,
        Slug = team.Slug,
        DateCreated = team.DateCreated,
        OrganizationId = team.OrganizationId,
        MemberIds = team.Members.Select(m => m.MemberId.Value).ToList()
    };
}

public sealed record TeamResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("dateCreated")]
    public DateTime DateCreated { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    [JsonPropertyName("memberIds")]
    public List<Guid> MemberIds { get; init; } = [];
}
