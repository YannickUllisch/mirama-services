using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.RemovePolicyStatement;

public class RemovePolicyStatementController : TenantControllerBase
{
    [HttpDelete("policies/{policyId:guid}/statements/{statementId:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid policyId, [FromRoute] Guid statementId)
    {
        var result = await this.Dispatcher.Send(new RemovePolicyStatementCommand(policyId, statementId));
        return result.Match(_ => NoContent(), Problem);
    }
}

public sealed record RemovePolicyStatementCommand(Guid PolicyId, Guid StatementId) : ICommand<ErrorOr<Deleted>>;

internal class RemovePolicyStatementCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<RemovePolicyStatementCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemovePolicyStatementCommand request, CancellationToken ct)
    {
        var policy = await dbContext.Policies
            .Include(p => p.Statements)
            .FirstOrDefaultAsync(p => p.Id.Value == request.PolicyId, ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        if (policy.TenantId is null || policy.IsManaged)
            return Error.Forbidden("Policy.SystemPolicy", "System policies cannot be modified.");

        if (policy.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Policy.Ownership", "You can only modify policies in your tenant.");

        var removeResult = policy.RemoveStatement(new PolicyStatementId(request.StatementId));
        if (removeResult.IsError) return removeResult.Errors;

        return Result.Deleted;
    }
}
