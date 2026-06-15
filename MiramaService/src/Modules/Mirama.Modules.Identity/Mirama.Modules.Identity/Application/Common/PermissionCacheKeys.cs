namespace Mirama.Modules.Identity.Application.Common;

internal static class PermissionCacheKeys
{
    internal static string RolePerms(Guid roleId) => $"rolePerms:{roleId}";
    internal static string MemberRoles(Guid userId, Guid orgId) => $"memberRoles:{userId}:{orgId}";
}
