
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Server.Common.Types;

public static class ScopeType
{
    public const string OpenId = Scopes.OpenId;
    public const string Email = Scopes.Email;
    public const string Roles = Scopes.Roles;
    public const string Profile = Scopes.Profile;
    public const string OfflineAccess = Scopes.OfflineAccess;
    public const string Tenant = "tenant";
    public const string Organization = "organization";
    public const string AccountRead = "account.read";
    public const string AccountWrite = "account.write";
    public const string ProjectRead = "project.read";
    public const string ProjectWrite = "project.write";
    public const string LLMRead = "llm.read";
    public const string LLMWrite = "llm.write";
}
