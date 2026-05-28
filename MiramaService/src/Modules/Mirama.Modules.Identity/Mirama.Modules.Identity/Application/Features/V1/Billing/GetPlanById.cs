using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Billing;

public class GetPlanByIdController : TenantControllerBase
{
    [HttpGet("plans/{id:guid}")]
    public async Task<ActionResult<PlanResponse>> Get([FromRoute] Guid id)
    {
        var res = await this.Dispatcher.Send(new GetPlanByIdQuery(id));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetPlanByIdQuery(Guid Id) : IQuery<ErrorOr<PlanResponse>>;

internal class GetPlanByIdQueryHandler(
    IIdentityQueryRepository<Plan, PlanId> planRepository) : IRequestHandler<GetPlanByIdQuery, ErrorOr<PlanResponse>>
{
    public async Task<ErrorOr<PlanResponse>> HandleAsync(GetPlanByIdQuery request, CancellationToken ct)
    {
        var plan = await planRepository.Query()
            .FirstOrDefaultAsync(p => p.Id.Value == request.Id, ct);

        if (plan is null)
            return Error.NotFound("Plan.NotFound", "Plan not found.");

        return plan.MapResponse();
    }
}
