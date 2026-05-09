using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.AddPolicyStatement;

public class AddPolicyStatementController : TenantControllerBase
{
    [HttpPost("policies/{policyId:guid}/statements")]
    public async Task<ActionResult<PolicyResponse>> Add([FromRoute] Guid policyId, [FromBody] AddPolicyStatementCommand command)
    {
        var result = await this.Dispatcher.Send(command with { PolicyId = policyId });
        return result.Match(Ok, Problem);
    }
}

public sealed record AddPolicyStatementCommand : ICommand<ErrorOr<PolicyResponse>>
{
    [JsonPropertyName("policyId")]
    public Guid PolicyId { get; init; }

    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("resource")]
    public string Resource { get; init; } = "*";

    [JsonPropertyName("effect")]
    public string Effect { get; init; } = "Allow";
}

internal class AddPolicyStatementCommandValidator : AbstractValidator<AddPolicyStatementCommand>
{
    public AddPolicyStatementCommandValidator()
    {
        RuleFor(c => c.Action).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Resource).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Effect)
            .Must(e => Enum.TryParse<Effect>(e, ignoreCase: true, out _))
            .WithMessage("Effect must be 'Allow' or 'Deny'");
    }
}

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
