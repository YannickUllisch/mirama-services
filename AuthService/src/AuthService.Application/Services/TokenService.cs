
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Domain.Authorization;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims.Interfaces;
using AuthService.Application.Domain.Scopes.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Services;

public sealed class TokenService(
    IAuthorizationContextFactory contextFactory,
    IScopePolicyPipeline scopePipeline,
    IClaimsPipeline claimsPipeline
) : ITokenService
{
    private readonly IAuthorizationContextFactory _contextFactory = contextFactory;
    private readonly IScopePolicyPipeline _scopePipeline = scopePipeline;
    private readonly IClaimsPipeline _claimsPipeline = claimsPipeline;

    /// <summary>
    /// Handles all token flows in a unified way:
    /// AuthorizationCode, RefreshToken, ClientCredentials, TokenExchange
    /// </summary>
    public async Task<AuthorizationResult> HandleAsync(HttpContext httpContext, AuthenticateResult result)
    {
        // Build the context based on the flow
        var context = await _contextFactory.CreateForTokenAsync(httpContext, result);
        if (context.IsRejected)
        {
            return AuthorizationResult.Deny(
                context.Error!,
                context.ErrorDescription!);
        }

        // Apply scope rules
        var decision = _scopePipeline.Evaluate(context);
        if (decision.IsDenied)
        {
            return AuthorizationResult.Deny(
                decision.Error!,
                decision.ErrorDescription!);
        }

        // Build claims principal
        var principal = await _claimsPipeline.BuildAsync(decision.Context);

        // Return for OpenIddict to sign
        return AuthorizationResult.Success(principal);
    }
}
