namespace Mirama.Modules.Identity.Contracts.Organizations;

public sealed record MemberDto(
    Guid Id,
    Guid OrganizationId,
    Guid UserId,
    string Name,
    string Email,
    IReadOnlyList<Guid> RoleIds);
