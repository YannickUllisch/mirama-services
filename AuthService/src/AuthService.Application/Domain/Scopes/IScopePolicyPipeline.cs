
using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Scopes;

public interface IScopePolicyPipeline
{
    AuthorizationDecision Evaluate(IAuthorizationContext context);
}
