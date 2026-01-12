
using System.Security.Claims;
using AuthService.Application.Domain.Authorization.Interfaces;
using AuthService.Application.Domain.Claims.Interfaces;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthService.Application.Domain.Claims.Contributors;

public sealed class SubjectClaimContributor : IClaimContributor
{
    public bool IsApplicable(IAuthorizationContext context) => true;

    public Task Contribute(IAuthorizationContext context, ClaimsIdentity identity)
    {
        var subject = context.GrantType switch
        {
            GrantTypes.ClientCredentials =>
                context.ClientId,

            GrantTypes.TokenExchange when context.Delegation is not null =>
                context.Delegation.DelegatedSubjectId,

            _ =>
                context.AuthenticatedUser!.UserId.ToString()
        };

        identity.SetClaim(ClaimType.Subject, subject);

        return Task.CompletedTask;
    }
}
