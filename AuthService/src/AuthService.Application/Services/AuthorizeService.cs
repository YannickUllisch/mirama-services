
using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Interfaces.Services;
using AuthService.Application.Domain.Claims;
using AuthService.Application.Domain.Scopes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AuthService.Application.Services;

public sealed class AuthorizeService(
    IAuthorizationContextFactory contextFactory,
    IScopePolicyPipeline scopePipeline,
    IClaimsPipeline claimsPipeline) 
    : IAuthorizeService
{
    private readonly IAuthorizationContextFactory _contextFactory = contextFactory;
    private readonly IScopePolicyPipeline _scopePipeline = scopePipeline;
    private readonly IClaimsPipeline _claimsPipeline = claimsPipeline;


    public async Task<AuthorizationResult> HandleAsync(HttpContext httpContext, AuthenticateResult result)
    {
        var context = await _contextFactory.CreateForAuthorizeAsync(httpContext, result);

        // Actual validation logic inside the different scope rules
        var decision = _scopePipeline.Evaluate(context);
        if (decision.IsDenied)
        {
            return AuthorizationResult.Deny(
                decision.Error!,
                decision.ErrorDescription!);
        }

        var principal = await _claimsPipeline.BuildAsync(decision.Context);

        return AuthorizationResult.Success(principal);
    }
}