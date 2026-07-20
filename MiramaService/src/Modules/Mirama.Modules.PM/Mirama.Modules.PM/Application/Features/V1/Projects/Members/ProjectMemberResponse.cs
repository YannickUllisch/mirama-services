using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members;

internal static class ProjectMemberMapper
{
    internal static ProjectMemberResponse ToResponse(ProjectMember member, MemberDto memberDto) => new()
    {
        Id = member.Id.Value,
        MemberId = member.MemberId,
        UserId = memberDto.UserId,
        Name = memberDto.Name,
        Email = memberDto.Email,
        RoleId = member.RoleId,
        IsInherited = member.IsInherited,
        TeamId = member.TeamId
    };
}

public sealed record ProjectMemberResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("memberId")]
    public Guid MemberId { get; init; }

    [JsonPropertyName("userId")]
    public Guid UserId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("roleId")]
    public Guid RoleId { get; init; }

    [JsonPropertyName("isInherited")]
    public bool IsInherited { get; init; }

    [JsonPropertyName("teamId")]
    public Guid? TeamId { get; init; }
}
