
namespace AuthService.Application.Domain.Scopes;

public static class ScopeType
{
    public const string OpenId = "openid";
    public const string Email = "email";
    public const string Roles = "roles";
    public const string Profile = "profile";
    public const string OfflineAccess = "offline_access";
    public const string Tenant = "tenant";
    public const string Organization = "organization";
    public const string AccountRead = "account.read";
    public const string AccountWrite = "account.write";
    public const string ProjectRead = "project.read";
    public const string ProjectWrite = "project.write";
    public const string LLMRead = "llm.read";
    public const string LLMWrite = "llm.write";
}
