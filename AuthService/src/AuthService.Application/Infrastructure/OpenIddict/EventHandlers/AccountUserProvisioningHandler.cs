using System.Security.Claims;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace AuthService.Application.Infrastructure.OpenIddict.EventHandlers;


public class AccountUserProvisioningHandler : IOpenIddictServerHandler<ProcessSignInContext>
{
    // private readonly IAccountService _accountService = accountService;

    public ValueTask HandleAsync(ProcessSignInContext context)
    {
        var principal = context.Principal;
        var identity = (ClaimsIdentity)principal!.Identity!;
        var scopes = principal.GetScopes();


        return ValueTask.CompletedTask;
    }
}