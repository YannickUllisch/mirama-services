using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Plan;

public sealed class Plan : AggregateRoot<PlanId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Price { get; private set; }
    public string Interval { get; private set; } = string.Empty;
    public IReadOnlyList<string> Features { get; private set; } = [];

    private Plan(PlanDetails details)
    {
        Name = details.Name.Trim();
        Description = details.Description?.Trim();
        Price = details.Price;
        Interval = details.Interval.Trim();
        Features = details.Features;
    }

    private Plan() { }

    public static Plan Create(PlanDetails details)
    {
        return new Plan(details) { Id = new PlanId(Guid.NewGuid()) };
    }

    public void Update(PlanDetails details)
    {
        Name = details.Name.Trim();
        Description = details.Description?.Trim();
        Price = details.Price;
        Interval = details.Interval.Trim();
        Features = details.Features;
    }
}
