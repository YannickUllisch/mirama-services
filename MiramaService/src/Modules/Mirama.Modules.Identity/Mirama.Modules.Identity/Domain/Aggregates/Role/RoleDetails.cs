using Mirama.Modules.Identity.Domain.Enums;

namespace Mirama.Modules.Identity.Domain.Aggregates.Role;

public sealed record RoleDetails(
    string Name,
    AccessScope Scope,
    Guid? TenantId = null,
    string? Description = null
);
