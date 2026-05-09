using Mirama.Modules.Identity.Domain.Aggregates.Plan;

namespace Mirama.Modules.Identity.Application.Features.V1.Plans;

internal static class PlanMapper
{
    internal static PlanResponse MapResponse(this Plan plan) => new()
    {
        Id = plan.Id.Value,
        Name = plan.Name,
        Description = plan.Description,
        Price = plan.Price,
        Interval = plan.Interval,
        Features = [.. plan.Features],
    };
}
