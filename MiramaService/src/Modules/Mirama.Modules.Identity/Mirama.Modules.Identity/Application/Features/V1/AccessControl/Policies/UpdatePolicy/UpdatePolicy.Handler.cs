using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.UpdatePolicy;

public class UpdatePolicyController : TenantControllerBase
{
    [RequirePermission(Permissions.IamPolicy.Manage)]
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
            .FirstOrDefaultAsync(p => p.Id == new PolicyId(request.Id), ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        if (policy.TenantId is null || policy.IsManaged)
            return Error.Forbidden("Policy.SystemPolicy", "System policies cannot be modified.");

        if (policy.TenantId != contextProvider.TenantId)
            return Error.Forbidden("Policy.Ownership", "You can only modify policies in your tenant.");

        policy.Update(request.Name, request.Description);

        var errors = new List<Error>();

        foreach (var statementId in request.RemoveStatementIds)
        {
            var result = policy.RemoveStatement(new PolicyStatementId(statementId));
            if (result.IsError) errors.AddRange(result.Errors);
        }

        foreach (var s in request.AddStatements)
        {
            Enum.TryParse<Effect>(s.Effect, ignoreCase: true, out var effect);
            var result = policy.AddStatement(s.Action, s.Resource, effect);
            if (result.IsError) errors.AddRange(result.Errors);
        }

        if (errors.Count > 0) return errors;

        return policy.MapResponse();
    }
}
