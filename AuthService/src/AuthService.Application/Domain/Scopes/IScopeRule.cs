
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Scopes;

public interface IScopeRule : IAuthorizationRule
{
    ScopeRulePhase Phase { get; }
    public IEnumerable<string> SupportedGrantTypes { get; }
}
