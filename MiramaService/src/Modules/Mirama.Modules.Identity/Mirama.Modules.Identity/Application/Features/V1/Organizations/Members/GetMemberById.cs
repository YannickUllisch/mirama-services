using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;

public class GetMemberByIdController : OrganizationControllerBase
{
    [HttpGet("members/{memberId:guid}")]
    public async Task<ActionResult<MemberResponse>> Get([FromRoute] Guid memberId)
    {
        var res = await this.Dispatcher.Send(new GetMemberByIdQuery(memberId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<ErrorOr<MemberResponse>>;

internal class GetMemberByIdQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetMemberByIdQuery, ErrorOr<MemberResponse>>
{
    public async Task<ErrorOr<MemberResponse>> HandleAsync(GetMemberByIdQuery request, CancellationToken ct)
    {
        var member = await dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == new MemberId(request.MemberId), ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        return member.MapResponse();
    }
}
