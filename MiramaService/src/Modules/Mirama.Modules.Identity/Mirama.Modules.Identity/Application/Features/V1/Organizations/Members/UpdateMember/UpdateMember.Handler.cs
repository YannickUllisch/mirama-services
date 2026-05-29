using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.UpdateMember;

public class UpdateMemberController : OrganizationControllerBase
{
    [HttpPatch("members/{memberId:guid}")]
    public async Task<ActionResult<MemberResponse>> Update([FromRoute] Guid memberId, [FromBody] UpdateMemberCommand command)
    {
        var result = await this.Dispatcher.Send(command with { MemberId = memberId });
        return result.Match(Ok, Problem);
    }
}

internal class UpdateMemberCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<UpdateMemberCommand, ErrorOr<MemberResponse>>
{
    public async Task<ErrorOr<MemberResponse>> HandleAsync(UpdateMemberCommand request, CancellationToken ct)
    {
        var member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == new MemberId(request.MemberId), ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        var roleExists = await dbContext.Roles
            .AsNoTracking()
            .AnyAsync(r => r.Id == new RoleId(request.IamRoleId), ct);

        if (!roleExists)
            return Error.NotFound("Member.Role.NotFound", "Role not found.");

        member.SetRole(new RoleId(request.IamRoleId));

        return member.MapResponse();
    }
}
