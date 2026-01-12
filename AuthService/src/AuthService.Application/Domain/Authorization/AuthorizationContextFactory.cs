
using System.Security.Claims;
using AuthService.Application.Common.Extension;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims;
using AuthService.Application.Infrastructure.HttpServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Authorization;

public sealed class AuthorizationContextFactory(IOpenIddictApplicationManager applicationManager, IAccountHttpClient accountClient) : IAuthorizationContextFactory
{
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly IAccountHttpClient _accountClient = accountClient;

    public async Task<IAuthorizationContext> CreateForAuthorizeAsync(HttpContext httpContext, AuthenticateResult result)
    {
        var oidcRequest = GetOidcRequest(httpContext);
        var principal = result.Principal!;

        var orgId = httpContext.Request.Query["organization_id"].FirstOrDefault();

        var user = await _accountClient.GetOrCreateUserAsync(
            accountId: principal.GetRequiredClaim(ClaimTypes.NameIdentifier, "sub"),
            email: principal.GetRequiredClaim(ClaimTypes.Email, "email"),
            name: principal.GetRequiredClaim(ClaimTypes.Name, "name"),
            address: "addressString", // TODO
            image: principal.GetOptionalClaim("picture"),
            orgId: orgId
        );

        var scopes = oidcRequest.GetScopes().ToHashSet();

        return new AuthorizationContext
        {
            Subject = httpContext.User,
            GrantType = oidcRequest.GrantType!,
            OrganizationId = user.OrganizationId,
            TenantId = user.TenantId,
            RequestedScopes = scopes,
            GrantedScopes = scopes,
            AuthenticatedUser = user,
        };
    }

    public async Task<IAuthorizationContext> CreateForTokenAsync(HttpContext httpContext, AuthenticateResult result)
    {
        var oidcRequest = GetOidcRequest(httpContext);

        // Values might be set if using Authorization Code, Refresh Token or Exchange Token Grants
        string? tenantId = result.Principal!.FindFirst(ClaimType.Tenant)?.Value;
        string? orgId = result.Principal!.FindFirst(ClaimType.Organization)?.Value;

        var scopes = oidcRequest.GetScopes().ToHashSet();

        var context = new AuthorizationContext
        {
            Subject = httpContext.User,
            GrantType = oidcRequest.GrantType!,
            RequestedScopes = scopes,
            GrantedScopes = scopes,
            TenantId = tenantId,
            OrganizationId = orgId,
        };

        switch (oidcRequest.GrantType)
        {
            case GrantTypes.AuthorizationCode:
                await PopulateFromUserPrincipal(context, result.Principal);
                break;

            case GrantTypes.RefreshToken:
                await PopulateFromUserPrincipal(context, result.Principal);
                break;

            case GrantTypes.ClientCredentials:
                await PopulateFromClientCredentials(context, oidcRequest);
                break;

            case GrantTypes.TokenExchange:
                await PopulateFromTokenExchange(context, result, oidcRequest);
                break;

            default:
                context.Reject(
                    Errors.UnsupportedGrantType,
                    "Unsupported grant type.");
                break;
        }

        return context;
    }


    private async Task PopulateFromUserPrincipal(AuthorizationContext context, ClaimsPrincipal principal)
    {
        // Set to previously calculated scopes
        context.GrantedScopes = principal.GetScopes().ToHashSet();

        if (context.TenantId is null)
        {
            context.Reject(
                Errors.InvalidGrant,
                "TenantId is missing for token generation.");
            return;
        }

        var userId = principal.FindFirst(ClaimType.Subject)?.Value;
        if (userId is null)
        {
            context.Reject(
                Errors.InvalidGrant,
                "Subject claim is missing.");
            return;
        }

        context.AuthenticatedUser = await _accountClient.GetUserAsync(userId, context.OrganizationId);
    }

    private async Task PopulateFromTokenExchange(AuthorizationContext context, AuthenticateResult result, OpenIddictRequest request)
    {
        var principal = result.Principal!;
        context.GrantedScopes = principal.GetScopes().ToHashSet();

        if (context.TenantId == null && context.ClientId == null)
        {
            context.Reject(
                Errors.InvalidGrant,
                "Either tenant or client must be present for token exchange.");
            return;
        }


        string? userId = (result.Principal!.FindFirst(ClaimType.Subject)?.Value)
            ?? throw new MissingFieldException("UserId is missing for Token generation");


        context.AuthenticatedUser = await _accountClient.GetUserAsync(userId, context.OrganizationId);

        // TODO: Write full token exchange handler with delegation logic
    }

    private async Task PopulateFromClientCredentials(AuthorizationContext context, OpenIddictRequest request)
    {
        // TODO: Write frull ClientCreds handler
        var application = await _applicationManager.FindByClientIdAsync(request.ClientId!)
            ?? throw new InvalidOperationException("The application cannot be found.");

        context.ClientId = await _applicationManager.GetClientIdAsync(application);
    }


    private static OpenIddictRequest GetOidcRequest(HttpContext context) =>
        context.GetOpenIddictServerRequest()
        ?? throw new InvalidOperationException("Invalid OpenID Connect request.");

}
