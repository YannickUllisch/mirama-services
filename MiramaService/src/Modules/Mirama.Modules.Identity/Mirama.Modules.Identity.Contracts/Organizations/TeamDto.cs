namespace Mirama.Modules.Identity.Contracts.Organizations;

public sealed record TeamDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Slug,
    IReadOnlyList<Guid> MemberIds);
