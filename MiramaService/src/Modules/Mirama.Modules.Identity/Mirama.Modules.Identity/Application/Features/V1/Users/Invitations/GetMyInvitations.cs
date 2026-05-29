using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.Invitations;

public class GetMyInvitationsController : ApiControllerBase
{
    [HttpGet("users/invitations")]
    public async Task<ActionResult<List<InvitationResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetMyInvitationsQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMyInvitationsQuery : IQuery<ErrorOr<List<InvitationResponse>>>;

internal class GetMyInvitationsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetMyInvitationsQuery, ErrorOr<List<InvitationResponse>>>
{
    public async Task<ErrorOr<List<InvitationResponse>>> HandleAsync(GetMyInvitationsQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == new UserId(contextProvider.UserId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var invitations = await dbContext.Invitations
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(i => i.Email == user.Email && i.Status == InvitationStatus.Pending && i.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(i => i.ExpiresAt)
            .ToListAsync(ct);

        return invitations.ConvertAll(i => new InvitationResponse
        {
            Id = i.Id.Value,
            Email = i.Email,
            Name = i.Name,
            InviterId = i.InviterId,
            IamRoleId = i.IamRoleId.Value,
            Status = i.Status.ToString(),
            ExpiresAt = i.ExpiresAt,
            OrganizationId = i.OrganizationId,
        });
    }
}
