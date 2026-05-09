using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;

internal static class RoleMapper
{
    internal static RoleResponse MapResponse(this Role role) => new()
    {
        Id = role.Id.Value,
        Name = role.Name,
        Description = role.Description,
        Scope = role.Scope.ToString(),
        IsSystemRole = role.IsSystemRole,
        PolicyIds = role.Policies.ConvertAll(p => p.Value),
    };
}
