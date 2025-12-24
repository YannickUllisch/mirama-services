using OpenIddict.Server;
using OpenIddict.Abstractions;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace AuthService.Server.Common.EventHandlers;

public class AccountUserProvisioningHandler() : IOpenIddictServerHandler<ProcessSignInContext>
{
    // private readonly IAccountService _accountService = accountService;

    public async ValueTask HandleAsync(ProcessSignInContext context)
    {
        var principal = context.Principal;
        // var userId = principal!.GetClaim(OpenIddictConstants.Claims.Subject);
        // var email = principal!.GetClaim(OpenIddictConstants.Claims.Email);


        // var userInfo = await _accountService.CreateIfNotExists(userId, email);
    }
}