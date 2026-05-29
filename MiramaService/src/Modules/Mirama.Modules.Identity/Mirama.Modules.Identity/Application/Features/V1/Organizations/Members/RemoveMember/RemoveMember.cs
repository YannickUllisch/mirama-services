using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.RemoveMember;

public class RemoveMemberController : OrganizationControllerBase
{
    [HttpDelete("members/{memberId:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid memberId)
    {
        var result = await this.Dispatcher.Send(new RemoveMemberCommand(memberId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record RemoveMemberCommand(Guid MemberId) : ICommand<ErrorOr<Deleted>>;

internal class RemoveMemberCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<RemoveMemberCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveMemberCommand request, CancellationToken ct)
    {
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == new MemberId(request.MemberId), ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        dbContext.Members.Remove(member);
        return Result.Deleted;
    }
}
