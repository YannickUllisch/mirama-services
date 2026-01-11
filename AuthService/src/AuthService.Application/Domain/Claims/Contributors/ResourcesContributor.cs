
using System.Collections.Immutable;
using System.Security.Claims;
using AuthService.Application.Common.Interfaces;
using OpenIddict.Abstractions;

namespace AuthService.Application.Domain.Claims.Contributors;

public sealed class ResourcesContributor(IOpenIddictScopeManager scopeManager) : IClaimContributor
{
    private readonly IOpenIddictScopeManager _scopeManager = scopeManager;

    public bool IsApplicable(IAuthorizationContext context)
        => context.GrantedScopes.Any();

    public async Task Contribute(IAuthorizationContext context, ClaimsIdentity identity)
    {
        var resources = await _scopeManager.ListResourcesAsync(context.GrantedScopes.ToImmutableArray()).ToListAsync();
        identity.SetResources(resources);
    }
}
