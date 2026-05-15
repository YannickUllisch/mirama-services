using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Clients.Application.Features.V1.Portal.InviteContact;

public class InviteContactController : OrganizationControllerBase
{
    [HttpPost("/clients/{clientId:guid}/portal/invitations")]
    public async Task<IActionResult> Invite(
        [FromRoute] Guid clientId,
        [FromBody] InviteContactCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ClientId = clientId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class InviteContactCommandHandler(
    IClientsCommandRepository<Client, ClientId> commandRepo,
    IClientsQueryRepository<Client, ClientId> queryRepo)
    : IRequestHandler<InviteContactCommand, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(InviteContactCommand request, CancellationToken cancellationToken)
    {
        var client = await queryRepo.Query()
            .Include(c => c.Contacts)
            .Include(c => c.PortalInvitations)
            .FirstOrDefaultAsync(c => c.Id.Value == request.ClientId, cancellationToken);

        if (client is null)
            return Error.NotFound("Client.NotFound", "Client not found.");

        try
        {
            var invitation = client.InviteContact(request.ContactId);
            commandRepo.Update(client);

            return new InvitationResponse(invitation.Id.Value, invitation.ContactId, invitation.ExpiresAt);
        }
        catch (InvalidOperationException ex)
        {
            return Error.Validation("Invitation.Invalid", ex.Message);
        }
    }
}
