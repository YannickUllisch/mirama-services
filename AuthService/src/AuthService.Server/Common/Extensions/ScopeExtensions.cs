
using AuthService.Server.Common.Enums;

namespace AuthService.Server.Common.Extensions;

public static class ScopeExtensionTypeExtensions 
{
    public static string AsString(this ScopeExtensionType claimType) => claimType switch
    {
        ScopeExtensionType.Organization => "organization",
        ScopeExtensionType.Postman => "postman",
        ScopeExtensionType.Tenant => "tenant",
        _ => throw new ArgumentOutOfRangeException(nameof(claimType), claimType, null)
    };
}