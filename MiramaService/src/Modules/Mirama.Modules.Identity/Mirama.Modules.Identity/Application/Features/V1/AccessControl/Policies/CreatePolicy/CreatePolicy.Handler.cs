using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.CreatePolicy;

public class CreatePolicyController : TenantControllerBase
{
    [HttpPost("policies")]
    public async Task<ActionResult<PolicyResponse>> Create([FromBody] CreatePolicyCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(p => CreatedAtAction(nameof(Create), new { id = p.Id }, p), Problem);
    }
}

internal class CreatePolicyCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<CreatePolicyCommand, ErrorOr<PolicyResponse>>
{
    public async Task<ErrorOr<PolicyResponse>> HandleAsync(CreatePolicyCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Policy.Create.NoTenant", "Tenant context required.");

        if (!Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope))
            return Error.Validation("Policy.Scope.Invalid", "Invalid scope value.");

        var duplicate = await dbContext.Policies
            .AsNoTracking()
            .AnyAsync(p => p.TenantId == tenantId && p.Name == request.Name.Trim() && p.Scope == scope, ct);

        if (duplicate)
            return Error.Conflict("Policy.Duplicate", "A policy with this name and scope already exists.");

        var policy = Policy.Create(new PolicyDetails(request.Name, scope, tenantId, request.Description));

        foreach (var dto in request.Statements)
        {
            if (!Enum.TryParse<Effect>(dto.Effect, ignoreCase: true, out var effect))
                return Error.Validation("PolicyStatement.Effect.Invalid", $"Invalid effect value: {dto.Effect}");

            var result = policy.AddStatement(dto.Action, dto.Resource, effect);
            if (result.IsError) return result.Errors;
        }

        dbContext.Policies.Add(policy);

        return policy.MapResponse();
    }
}
