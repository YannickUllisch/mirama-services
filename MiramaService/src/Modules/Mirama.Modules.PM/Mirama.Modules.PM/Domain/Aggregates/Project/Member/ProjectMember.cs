using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Project.Member;

public sealed class ProjectMember : OrganizationEntity<ProjectMemberId>
{
    public Guid MemberId { get; private set; }
    public Guid RoleId { get; private set; }
    public bool IsInherited { get; private set; }
    public Guid? TeamId { get; private set; }

    private ProjectMember(ProjectMemberDetails details)
    {
        this.MemberId = details.MemberId;
        this.RoleId = details.RoleId;
        this.IsInherited = details.IsInherited;
        this.TeamId = details.TeamId;
    }

    private ProjectMember() { }

    internal static ProjectMember Create(ProjectMemberDetails details) =>
        new(details) { Id = new ProjectMemberId(Guid.NewGuid()) };

    internal static ProjectMember CreateInherited(Guid memberId, Guid roleId, Guid teamId) =>
        new(new ProjectMemberDetails(memberId, roleId, IsInherited: true, TeamId: teamId))
        {
            Id = new ProjectMemberId(Guid.NewGuid())
        };

    public void SetDirectAssignment(Guid roleId)
    {
        this.RoleId = roleId;
        this.IsInherited = false;
        this.TeamId = null;
    }

    public void UpdateRole(Guid roleId)
    {
        this.RoleId = roleId;
    }
}
