using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Plan;

public sealed class Plan : AggregateRoot<PlanId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Price { get; private set; } // in cents
    public string Interval { get; private set; } = string.Empty;
    public IReadOnlyList<string> Features { get; private set; } = [];

    private Plan() { }

    public static Plan Create(string name, string? description, int price, string interval, IReadOnlyList<string> features)
    {
        return new Plan
        {
            Id = new PlanId(Guid.NewGuid()),
            Name = name.Trim(),
            Description = description?.Trim(),
            Price = price,
            Interval = interval.Trim(),
            Features = features
        };
    }

    public void Update(string name, string? description, int price, string interval, IReadOnlyList<string> features)
    {
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        Interval = interval.Trim();
        Features = features;
    }
}
