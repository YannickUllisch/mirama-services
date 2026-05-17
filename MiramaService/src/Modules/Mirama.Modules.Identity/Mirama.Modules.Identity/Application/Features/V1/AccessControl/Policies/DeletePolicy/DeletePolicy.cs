using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.DeletePolicy;

public class DeletePolicyController : TenantControllerBase
{
    [HttpDelete("policies/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await this.Dispatcher.Send(new DeletePolicyCommand(id));
        return result.Match(ToNoContent, Problem);
    }
}

public sealed record DeletePolicyCommand(Guid Id) : ICommand<ErrorOr<Deleted>>;

internal class DeletePolicyCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<DeletePolicyCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeletePolicyCommand request, CancellationToken ct)
    {
        var policy = await dbContext.Policies
            .FirstOrDefaultAsync(p => p.Id.Value == request.Id, ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        if (policy.TenantId is null || policy.IsManaged)
            return Error.Forbidden("Policy.SystemPolicy", "System policies cannot be deleted.");

        if (policy.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Policy.Ownership", "You can only delete policies in your tenant.");

        dbContext.Policies.Remove(policy);

        return Result.Deleted;
    }
}
