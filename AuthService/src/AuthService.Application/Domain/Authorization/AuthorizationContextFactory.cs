
using AuthService.Application.Domain.Authentication;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Authorization;

public sealed class AuthorizationContextFactory(IOpenIddictApplicationManager applicationManager) : IAuthorizationContextFactory
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;

    public Task<IAuthorizationContext> CreateForAuthorizeAsync(HttpContext httpContext, AuthenticateResult result)
    {
        var oidcRequest = httpContext.GetOpenIddictServerRequest() 
            ?? throw new InvalidOperationException("Invalid OpenID Connect request.");;

        // Trying to extract OrgId if passed as query param
        Guid? orgId = null;
        var orgIdString = httpContext.Request.Query["organization_id"].FirstOrDefault();
        if (Guid.TryParse(orgIdString, out var parsedOrgId))
        {
            orgId = parsedOrgId;
        }

        // TODO: Fetch properly
        // var accountId = authPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        // var userInfo = await _accountService.GetUserAsync(accountId);

        // Add User Details - fetch from actual HTTPClient later
        var user = new AuthenticatedUser
        {
            UserId = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            OrganizationId = orgId == null ? orgId : Guid.NewGuid(),
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        };

        return Task.FromResult<IAuthorizationContext>(new AuthorizationContext
        {
            Subject = httpContext.User,
            GrantType = oidcRequest.GrantType!,
            OrganizationId = orgId,
            RequestedScopes = oidcRequest.GetScopes().ToHashSet(),
            GrantedScopes = oidcRequest.GetScopes().ToHashSet(),
            AuthenticatedUser = user,
        });
    }

    public async Task<IAuthorizationContext> CreateForTokenAsync(HttpContext httpContext, AuthenticateResult result)
    {
        var oidcRequest = httpContext.GetOpenIddictServerRequest() 
            ?? throw new InvalidOperationException("Invalid OpenID Connect request.");;

        var context = new AuthorizationContext
        {
            GrantType = oidcRequest.GrantType!,
            RequestedScopes = oidcRequest.GetScopes().ToHashSet(),
            GrantedScopes = oidcRequest.GetScopes().ToHashSet()
        };

        switch (oidcRequest.GrantType)
        {
            case GrantTypes.AuthorizationCode:
                Console.WriteLine("HERE IN CORRECT AUTHCODE FLOW");
                await PopulateFromAuthorizationCode(context, result);
                break;

            case GrantTypes.RefreshToken:
                PopulateFromRefreshToken(context, result);
                break;

            case GrantTypes.ClientCredentials:
                await PopulateFromClientCredentials(context, oidcRequest);
                break;

            case GrantTypes.TokenExchange:
                PopulateFromTokenExchange(context, result, oidcRequest);
                break;

            default:
                context.Reject(
                    Errors.UnsupportedGrantType,
                    "Unsupported grant type.");
                break;
        }

        return context;
    }


    private Task PopulateFromAuthorizationCode(AuthorizationContext context, AuthenticateResult result)
    {
        var principal = result.Principal!;
        context.GrantedScopes = principal.GetScopes().ToHashSet();

        // Hardcoded user
        context.AuthenticatedUser = new AuthenticatedUser
        {
            UserId = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            OrganizationId = principal.FindFirst(ClaimType.Organization)?.Value is string orgStr && Guid.TryParse(orgStr, out var orgId) ? orgId : null,
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        };

        return Task.CompletedTask;
    }

    private void PopulateFromRefreshToken(AuthorizationContext context, AuthenticateResult result)
    {
        var principal = result.Principal!;
        context.GrantedScopes = principal.GetScopes().ToHashSet();

        // Hardcoded user
        context.AuthenticatedUser = new AuthenticatedUser
        {
            UserId = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            OrganizationId = principal.FindFirst(ClaimType.Organization)?.Value is string orgStr && Guid.TryParse(orgStr, out var orgId) ? orgId : null,
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        };
    }


    private async Task PopulateFromClientCredentials(AuthorizationContext context, OpenIddictRequest request)
    {
        var application = await _applicationManager.FindByClientIdAsync(request.ClientId!) ??
            throw new InvalidOperationException("The application cannot be found.");

        context.ClientId = await _applicationManager.GetClientIdAsync(application);
    }


    private void PopulateFromTokenExchange(AuthorizationContext context, AuthenticateResult result, OpenIddictRequest request)
    {
        var principal = result.Principal!;
        context.GrantedScopes = principal.GetScopes().ToHashSet();

        // Hardcoded delegated user for now
        context.AuthenticatedUser = new AuthenticatedUser
        {
            UserId = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            OrganizationId = request.GetParameter("organization_id")?.ToString() is string orgStr && Guid.TryParse(orgStr, out var orgId) ? orgId : null,
            Email = "user@example.com",
            Name = "John Doe",
            Role = "admin",
            IsActive = true,
            Image = "https://example.com/avatar.png"
        };

        // context.Delegation = new DelegationContext()... principal.FindFirst(Claims.Subject)?.Value;
        // context.ClientId = ...
    }
}
