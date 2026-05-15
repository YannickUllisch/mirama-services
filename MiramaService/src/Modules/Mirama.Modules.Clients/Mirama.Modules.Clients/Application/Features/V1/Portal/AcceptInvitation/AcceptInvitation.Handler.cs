using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Auth;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Clients.Application.Features.V1.Portal.AcceptInvitation;

[ApiController]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/portal/auth")]
public class AcceptInvitationController : ApiControllerBase
{
    [HttpPost("accept")]
    public async Task<IActionResult> Accept([FromBody] AcceptInvitationCommand command, CancellationToken ct)
    {
        var result = await Dispatcher.Send(command, ct);
        return result.Match(Ok, Problem);
    }
}

internal class AcceptInvitationCommandHandler(
    IClientsCommandRepository<Client, ClientId> commandRepo,
    IClientsQueryRepository<Client, ClientId> queryRepo,
    IClientPortalTokenService tokenService)
    : IRequestHandler<AcceptInvitationCommand, ErrorOr<PortalSessionResponse>>
{
    public async Task<ErrorOr<PortalSessionResponse>> HandleAsync(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var client = await queryRepo.Query()
            .Include(c => c.Contacts)
            .Include(c => c.PortalInvitations)
            .Include(c => c.PortalUsers)
            .FirstOrDefaultAsync(
                c => c.PortalInvitations.Any(i => i.Token == request.Token),
                cancellationToken);

        if (client is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        try
        {
            var portalUser = client.AcceptInvitation(request.Token);
            commandRepo.Update(client);

            var token = tokenService.GenerateSessionToken(
                portalUser.Id.Value,
                client.Id.Value,
                portalUser.ContactId,
                client.TenantId);

            return new PortalSessionResponse(portalUser.Id.Value, client.Id.Value, token);
        }
        catch (InvalidOperationException ex)
        {
            return Error.Conflict("Invitation.Invalid", ex.Message);
        }
    }
}
