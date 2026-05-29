using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.AddMember;

public class AddMemberController : OrganizationControllerBase
{
    [HttpPost("members")]
    public async Task<ActionResult<MemberResponse>> Add([FromBody] AddMemberCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Add), new { memberId = r.Id }, r), Problem);
    }
}

internal class AddMemberCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<AddMemberCommand, ErrorOr<MemberResponse>>
{
    public async Task<ErrorOr<MemberResponse>> HandleAsync(AddMemberCommand request, CancellationToken ct)
    {
        var organizationId = contextProvider.OrganizationId;
        if (organizationId is null)
            return Error.Unauthorized("Member.NoOrg", "Organization context required.");

        var roleExists = await dbContext.Roles
            .AsNoTracking()
            .AnyAsync(r => r.Id == new RoleId(request.IamRoleId), ct);

        if (!roleExists)
            return Error.NotFound("Member.Role.NotFound", "Role not found.");

        var duplicate = await dbContext.Members
            .AsNoTracking()
            .AnyAsync(m => m.Email == request.Email.Trim(), ct);

        if (duplicate)
            return Error.Conflict("Member.Duplicate", "A member with this email already exists in the organization.");

        var details = new MemberDetails(request.Name, request.Email, new RoleId(request.IamRoleId));
        var member = Member.Create(details);

        dbContext.Members.Add(member);

        return member.MapResponse() with { OrganizationId = organizationId.Value };
    }
}
