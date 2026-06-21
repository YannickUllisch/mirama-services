namespace Mirama.Modules.PM.Domain.Aggregates.Project.Member;

public sealed record ProjectMemberDetails(
    Guid MemberId,
    Guid RoleId,
    bool IsInherited = false,
    Guid? TeamId = null);
