using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;

public class Member : OrganizationEntity<MemberId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserId UserId { get; private set; } = default!;
    public List<RoleId> IamRoleIds { get; private set; } = [];

    private Member(MemberDetails details)
    {
        this.Name = details.Name.Trim();
        this.Email = details.Email.Trim();
        this.UserId = details.UserId;
        this.IamRoleIds = [details.IamRoleId];
    }

    private Member() { }

    public static Member Create(MemberDetails details)
        => new Member(details) { Id = new MemberId(Guid.NewGuid()) };

    public ErrorOr<Success> AssignRole(RoleId roleId)
    {
        if (this.IamRoleIds.Contains(roleId))
            return Error.Conflict("Member.Role.Duplicate", "Role already assigned.");
        this.IamRoleIds.Add(roleId);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveRole(RoleId roleId)
    {
        if (this.IamRoleIds.Count == 1)
            return Error.Validation("Member.Role.Required", "Member must have at least one role.");
        if (!this.IamRoleIds.Remove(roleId))
            return Error.NotFound("Member.Role.NotFound", "Role not assigned to this member.");
        return Result.Success;
    }

    public void SetRole(RoleId roleId)
    {
        this.IamRoleIds = [roleId];
    }

    public void LinkUser(UserId userId)
    {
        this.UserId = userId;
    }
}
