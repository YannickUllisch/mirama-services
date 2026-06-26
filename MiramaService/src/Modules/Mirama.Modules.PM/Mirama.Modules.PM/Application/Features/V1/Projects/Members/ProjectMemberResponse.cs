using Mirama.Modules.PM.Domain.Aggregates.Project.Member;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members;

public sealed record ProjectMemberResponse(
    Guid ProjectMemberId,
    Guid MemberId,
    Guid RoleId,
    bool IsInherited);

internal static class ProjectMemberMapper
{
    internal static ProjectMemberResponse ToResponse(ProjectMember member) =>
        new(member.Id.Value, member.MemberId, member.RoleId, member.IsInherited);
}
