
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Scopes;

public interface IScopeRule : IAuthorizationRule
{
    public IEnumerable<string> SupportedGrantTypes { get; }
}
