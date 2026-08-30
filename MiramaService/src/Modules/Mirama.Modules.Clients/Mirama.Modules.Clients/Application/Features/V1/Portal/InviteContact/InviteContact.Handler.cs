using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
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
    IClientsQueryRepository<Client, ClientId> queryRepo,
    IMemberService memberService,
    IRequestContextProvider context)
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

        var member = await memberService.GetMemberByUserIdAsync(context.OrganizationId!.Value, context.UserId, cancellationToken);
        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        try
        {
            var invitation = client.InviteContact(request.ContactId, request.Role, member.Id);
            commandRepo.Update(client);

            return new InvitationResponse(invitation.Id.Value, invitation.ContactId, invitation.ExpiresAt);
        }
        catch (InvalidOperationException ex)
        {
            return Error.Validation("Invitation.Invalid", ex.Message);
        }
    }
}
