using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Plans;

public class GetPlansController : TenantControllerBase
{
    [HttpGet("plans")]
    public async Task<ActionResult<List<PlanResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetPlansQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetPlansQuery : IQuery<ErrorOr<List<PlanResponse>>>;

internal class GetPlansQueryHandler(
    IIdentityQueryRepository<Plan, PlanId> planRepository) : IRequestHandler<GetPlansQuery, ErrorOr<List<PlanResponse>>>
{
    public async Task<ErrorOr<List<PlanResponse>>> HandleAsync(GetPlansQuery request, CancellationToken ct)
    {
        var plans = await planRepository.Query()
            .OrderBy(p => p.Price)
            .ToListAsync(ct);

        return plans.ConvertAll(p => p.MapResponse());
    }
}
