namespace Mirama.SharedKernel.Models.Permissions;

public static class PermissionMatcher
{
    public static bool IsGranted(IReadOnlySet<string> effective, string required)
    {
        if (effective.Contains(Permissions.Wildcard)) return true;
        if (effective.Contains(required)) return true;

        var colonIdx = required.IndexOf(':');
        if (colonIdx <= 0) return false;

        var resource = required[..colonIdx];
        var action = required[(colonIdx + 1)..];

        // "resource:*" grants all actions on that resource
        if (effective.Contains($"{resource}:*")) return true;

        // "*:action" (e.g. "*:read") grants that action across all resources
        if (effective.Contains($"*:{action}")) return true;

        return false;
    }
}
