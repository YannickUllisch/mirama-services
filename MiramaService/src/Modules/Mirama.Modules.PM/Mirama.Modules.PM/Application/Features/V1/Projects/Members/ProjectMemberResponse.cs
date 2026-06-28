using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members;

public sealed record ProjectMemberResponse(
    Guid ProjectMemberId,
    Guid MemberId,
    Guid UserId,
    string Name,
    string Email,
    Guid RoleId,
    bool IsInherited,
    Guid? TeamId);

internal static class ProjectMemberMapper
{
    internal static ProjectMemberResponse ToResponse(ProjectMember member, MemberDto memberDto) =>
        new(member.Id.Value, member.MemberId, memberDto.UserId, memberDto.Name, memberDto.Email, member.RoleId, member.IsInherited, member.TeamId);
}
