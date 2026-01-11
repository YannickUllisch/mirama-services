
using AuthService.Application.Domain.Authorization;
using AuthService.Application.Domain.Authorization.Interfaces;

namespace AuthService.Application.Domain.Scopes.Interfaces;

public interface IScopePolicyPipeline
{
    AuthorizationDecision Evaluate(IAuthorizationContext context);
}
