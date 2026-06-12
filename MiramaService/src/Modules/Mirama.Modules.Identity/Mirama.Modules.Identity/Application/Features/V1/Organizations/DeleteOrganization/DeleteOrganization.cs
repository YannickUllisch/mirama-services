using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.DeleteOrganization;

public class DeleteOrganizationController : TenantControllerBase
{
    [HttpDelete("organizations/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await this.Dispatcher.Send(new DeleteOrganizationCommand(id));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record DeleteOrganizationCommand(Guid Id) : ICommand<ErrorOr<Deleted>>;

internal class DeleteOrganizationCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<DeleteOrganizationCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteOrganizationCommand request, CancellationToken ct)
    {
        var org = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(request.Id), ct);

        if (org is null)
            return Error.NotFound("Organization.NotFound", "Organization not found.");

        dbContext.Organizations.Remove(org);
        return Result.Deleted;
    }
}
