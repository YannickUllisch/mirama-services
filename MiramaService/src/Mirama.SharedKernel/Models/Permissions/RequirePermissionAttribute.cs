namespace Mirama.SharedKernel.Models.Permissions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class RequirePermissionAttribute(params string[] permissions) : Attribute
{
    public string[] Permissions { get; } = permissions;
}
