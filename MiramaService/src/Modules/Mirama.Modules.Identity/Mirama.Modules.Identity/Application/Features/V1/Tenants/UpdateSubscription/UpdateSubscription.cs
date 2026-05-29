using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants.UpdateSubscription;

public class UpdateSubscriptionController : TenantControllerBase
{
    [HttpPut("subscription")]
    public async Task<ActionResult<TenantResponse>> Update([FromBody] UpdateSubscriptionCommand command)
    {
        var result = await this.Dispatcher.Send(command);
        return result.Match(Ok, Problem);
    }
}

public sealed record UpdateSubscriptionCommand : ICommand<ErrorOr<TenantResponse>>
{
    // When payment integration is added, this endpoint should initiate the plan change
    // with the payment provider (e.g. Stripe) and apply the subscription update only
    // after receiving a confirmed webhook. For now, plan changes are applied directly.

    [JsonPropertyName("planId")]
    public Guid PlanId { get; init; }

    [JsonPropertyName("periodStart")]
    public DateTime PeriodStart { get; init; }

    [JsonPropertyName("periodEnd")]
    public DateTime PeriodEnd { get; init; }
}

internal class UpdateSubscriptionCommandValidator : AbstractValidator<UpdateSubscriptionCommand>
{
    public UpdateSubscriptionCommandValidator()
    {
        RuleFor(c => c.PlanId).NotEmpty();
        RuleFor(c => c.PeriodStart).NotEmpty();
        RuleFor(c => c.PeriodEnd)
            .NotEmpty()
            .GreaterThan(c => c.PeriodStart)
            .WithMessage("PeriodEnd must be after PeriodStart.");
    }
}

internal class UpdateSubscriptionCommandHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<UpdateSubscriptionCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> HandleAsync(UpdateSubscriptionCommand request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;
        if (tenantId is null)
            return Error.Unauthorized("Tenant.NoContext", "Tenant context required.");

        var plan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Value == request.PlanId, ct);

        if (plan is null)
            return Error.NotFound("Tenant.Plan.NotFound", "Plan not found.");

        var tenant = await dbContext.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId.Value, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

        tenant.SetSubscription(new SubscriptionDetails(
            new PlanId(request.PlanId),
            request.PeriodStart,
            request.PeriodEnd));

        return tenant.MapResponse(plan);
    }
}
