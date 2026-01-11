

using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Domain.Scopes;

public sealed class ScopePolicyPipeline(IEnumerable<IScopeRule> rules) : IScopePolicyPipeline
{
    private readonly IEnumerable<IAuthorizationRule> _rules = rules
            .OrderBy(r => r.Phase)
            .ToList();

    public AuthorizationDecision Evaluate(IAuthorizationContext context)
    {
        foreach (var rule in _rules)
        {
            if (context.IsRejected)
            {
                break;
            }

            if (rule.IsApplicable(context))
            {
                rule.Apply(context);
            }
        }

        // If any rule rejected the Authorization Context, we propagate the rejection as decision
        if (context.IsRejected)
        {
            return AuthorizationDecision.Deny(
                context.Error!,
                context.ErrorDescription!);
        }

        return AuthorizationDecision.Success(context);
    }
}
