using Mirama.Modules.Identity.Domain.Enums;

namespace Mirama.Modules.Identity.Domain.Aggregates.Policy;

public sealed record PolicyDetails(
    string Name,
    AccessScope Scope,
    Guid? TenantId = null,
    string? Description = null,
    bool IsManaged = false
);
