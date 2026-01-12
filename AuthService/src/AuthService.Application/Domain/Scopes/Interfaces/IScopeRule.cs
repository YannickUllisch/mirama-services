
using AuthService.Application.Domain.Authorization.Interfaces;

namespace AuthService.Application.Domain.Scopes.Interfaces;

public interface IScopeRule
{
    ScopeRulePhase Phase { get; }
    public IEnumerable<string> SupportedGrantTypes { get; }

    bool IsApplicable(IAuthorizationContext context);
    void Apply(IAuthorizationContext context);
}
