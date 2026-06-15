using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl;

public class GetMemberPermissionsController : OrganizationControllerBase
{
    [HttpGet("members/{memberId:guid}/permissions")]
    public async Task<ActionResult<string[]>> Get([FromRoute] Guid memberId)
    {
        var res = await this.Dispatcher.Send(new GetMemberPermissionsQuery(memberId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMemberPermissionsQuery(Guid MemberId) : IQuery<ErrorOr<string[]>>;

internal class GetMemberPermissionsQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetMemberPermissionsQuery, ErrorOr<string[]>>
{
    public async Task<ErrorOr<string[]>> HandleAsync(GetMemberPermissionsQuery request, CancellationToken ct)
    {
        var member = await dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == new MemberId(request.MemberId), ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        if (member.IamRoleIds.Count == 0)
            return Array.Empty<string>();

        var roles = await dbContext.Roles
            .AsNoTracking()
            .Where(r => member.IamRoleIds.Contains(r.Id))
            .ToListAsync(ct);

        var allPolicyIds = roles.SelectMany(r => r.Policies).Distinct().ToList();
        if (allPolicyIds.Count == 0)
            return Array.Empty<string>();

        var statements = await dbContext.Policies
            .AsNoTracking()
            .Where(p => allPolicyIds.Contains(p.Id))
            .SelectMany(p => p.Statements)
            .ToListAsync(ct);

        var allows = statements.Where(s => s.Effect == Effect.Allow).Select(s => s.Action).ToHashSet();
        var denies = statements.Where(s => s.Effect == Effect.Deny).Select(s => s.Action).ToHashSet();

        allows.ExceptWith(denies);
        return allows.ToArray();
    }
}
