namespace Mirama.Modules.Workspace.Contracts;

/// <summary>
/// Read-only projection of a user's saved view state, for other modules that need
/// to look up personalization data synchronously (e.g. seeding a default on create).
/// </summary>
public sealed record ViewStateDto(
    Guid Id,
    Guid UserId,
    Guid OrganizationId,
    string SurfaceKey,
    string ViewType,
    string StateJson,
    DateTime? LastModified);
