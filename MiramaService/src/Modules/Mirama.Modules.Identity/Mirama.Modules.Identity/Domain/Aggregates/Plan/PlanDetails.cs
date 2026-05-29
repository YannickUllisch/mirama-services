namespace Mirama.Modules.Identity.Domain.Aggregates.Plan;

public sealed record PlanDetails(
    string Name,
    int Price,
    string Interval,
    IReadOnlyList<string> Features,
    string? Description = null
);
