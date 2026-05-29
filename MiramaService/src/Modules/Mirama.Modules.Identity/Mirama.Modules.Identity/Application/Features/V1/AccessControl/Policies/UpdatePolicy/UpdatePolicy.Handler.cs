using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.UpdatePolicy;

public class UpdatePolicyController : TenantControllerBase
{
    [HttpPut("policies/{id:guid}")]
    public async Task<ActionResult<PolicyResponse>> Update([FromRoute] Guid id, [FromBody] UpdatePolicyCommand command)
    {
        var result = await this.Dispatcher.Send(command with { Id = id });
        return result.Match(Ok, Problem);
    }
}

internal class UpdatePolicyCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdatePolicyCommand, ErrorOr<PolicyResponse>>
{
    public async Task<ErrorOr<PolicyResponse>> HandleAsync(UpdatePolicyCommand request, CancellationToken ct)
    {
        var policy = await dbContext.Policies
            .Include(p => p.Statements)
            .FirstOrDefaultAsync(p => p.Id.Value == request.Id, ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        if (policy.TenantId is null || policy.IsManaged)
            return Error.Forbidden("Policy.SystemPolicy", "System policies cannot be modified.");

        if (policy.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Policy.Ownership", "You can only modify policies in your tenant.");

        policy.Update(new PolicyDetails(request.Name, policy.Scope, policy.TenantId, request.Description));

        return policy.MapResponse();
    }
}
