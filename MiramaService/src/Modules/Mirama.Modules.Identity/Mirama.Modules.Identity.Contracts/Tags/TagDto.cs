namespace Mirama.Modules.Identity.Contracts.Tags;

public sealed record TagDto(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Slug,
    string? Color,
    string? Description,
    TagScopeDto Scope,
    DateTime DateCreated);
