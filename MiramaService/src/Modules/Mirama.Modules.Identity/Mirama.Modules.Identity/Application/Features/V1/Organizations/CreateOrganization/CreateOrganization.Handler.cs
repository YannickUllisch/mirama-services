using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.CreateOrganization;

public class CreateOrganizationController : TenantControllerBase
{
    [HttpPost("organizations")]
    public async Task<ActionResult<OrganizationResponse>> Create([FromBody] CreateOrganizationCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.Id }, r), Problem);
    }
}

internal class CreateOrganizationCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CreateOrganizationCommand, ErrorOr<OrganizationResponse>>
{
    public async Task<ErrorOr<OrganizationResponse>> HandleAsync(CreateOrganizationCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Organization.NoTenant", "Tenant context required.");

        var userId = contextProvider.UserId;

        var user = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == new UserId(userId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "Current user not found.");

        var ownerRole = await dbContext.Roles
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Name == "Owner" && r.TenantId == null, ct);

        if (ownerRole is null)
            return Error.Unexpected("Role.OwnerNotFound", "Owner role not found.");

        var details = new OrganizationDetails(request.Name, request.Street, request.City, request.Country, request.ZipCode, request.Logo);
        var org = Organization.Create(details);
        dbContext.Organizations.Add(org);

        var member = Member.Create(new MemberDetails(user.Name, user.Email, ownerRole.Id, new UserId(userId)));
        ((IOrganizationOwned)member).SetOrganizationId(org.Id.Value);
        dbContext.Members.Add(member);

        return org.MapResponse(memberCount: 1) with { TenantId = tenantId.Value, };
    }
}
