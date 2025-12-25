using System.Security.Claims;
using AuthService.Server.Common.Enums;
using AuthService.Server.Common.Extensions;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace AuthService.Server.Common.EventHandlers;

public class AccountUserProvisioningHandler : IOpenIddictServerHandler<ProcessSignInContext>
{
    // private readonly IAccountService _accountService = accountService;

    public ValueTask HandleAsync(ProcessSignInContext context)
    {
        var principal = context.Principal;
        var identity = (ClaimsIdentity)principal!.Identity!;
        var scopes = principal.GetScopes();

        Console.WriteLine(ScopeExtensionType.Organization.AsString(), ScopeExtensionType.Postman.AsString());

        return ValueTask.CompletedTask;
    }
}