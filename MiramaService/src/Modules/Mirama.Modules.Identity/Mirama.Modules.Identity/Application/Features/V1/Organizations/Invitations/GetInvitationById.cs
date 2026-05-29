using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;

public class GetInvitationByIdController : OrganizationControllerBase
{
    [HttpGet("invitations/{invitationId:guid}")]
    public async Task<ActionResult<InvitationResponse>> Get([FromRoute] Guid invitationId)
    {
        var res = await this.Dispatcher.Send(new GetInvitationByIdQuery(invitationId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetInvitationByIdQuery(Guid InvitationId) : IQuery<ErrorOr<InvitationResponse>>;

internal class GetInvitationByIdQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetInvitationByIdQuery, ErrorOr<InvitationResponse>>
{
    public async Task<ErrorOr<InvitationResponse>> HandleAsync(GetInvitationByIdQuery request, CancellationToken ct)
    {
        var invitation = await dbContext.Invitations
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == new InvitationId(request.InvitationId), ct);

        if (invitation is null)
            return Error.NotFound("Invitation.NotFound", "Invitation not found.");

        return invitation.MapResponse();
    }
}
