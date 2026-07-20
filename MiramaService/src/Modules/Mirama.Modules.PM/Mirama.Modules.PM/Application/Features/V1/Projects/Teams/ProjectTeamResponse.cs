using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Domain.Aggregates.Project.Team;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams;

internal static class ProjectTeamMapper
{
    internal static ProjectTeamResponse ToResponse(ProjectTeam projectTeam, TeamDto team) => new()
    {
        Id = projectTeam.Id.Value,
        TeamId = projectTeam.TeamId,
        Name = team.Name,
        Slug = team.Slug,
        MemberIds = team.MemberIds,
        DateAdded = projectTeam.DateAdded
    };
}

public sealed record ProjectTeamResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("teamId")]
    public Guid TeamId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; init; } = string.Empty;

    [JsonPropertyName("memberIds")]
    public IReadOnlyList<Guid> MemberIds { get; init; } = [];

    [JsonPropertyName("dateAdded")]
    public DateTime DateAdded { get; init; }
}
