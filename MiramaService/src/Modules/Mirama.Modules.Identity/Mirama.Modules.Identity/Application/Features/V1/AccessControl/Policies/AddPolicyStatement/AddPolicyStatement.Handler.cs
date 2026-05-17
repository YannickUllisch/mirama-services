using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.AddPolicyStatement;

internal class AddPolicyStatementCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<AddPolicyStatementCommand, ErrorOr<PolicyResponse>>
{
    public async Task<ErrorOr<PolicyResponse>> HandleAsync(AddPolicyStatementCommand request, CancellationToken ct)
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

        var duplicate = policy.Statements.Any(s =>
            s.Action.Equals(request.Action.Trim(), StringComparison.OrdinalIgnoreCase) &&
            s.Resource.Equals(request.Resource.Trim(), StringComparison.OrdinalIgnoreCase));

        if (duplicate)
            return Error.Conflict("PolicyStatement.Duplicate", "A statement with this action and resource already exists.");

        if (!Enum.TryParse<Effect>(request.Effect, ignoreCase: true, out var effect))
            return Error.Validation("PolicyStatement.Effect.Invalid", "Invalid effect value.");

        var result = policy.AddStatement(request.Action, request.Resource, effect);
        if (result.IsError) return result.Errors;

        return policy.MapResponse();
    }
}
