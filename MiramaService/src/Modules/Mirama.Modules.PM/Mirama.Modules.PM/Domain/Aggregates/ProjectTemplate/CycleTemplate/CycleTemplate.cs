using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;

public sealed class CycleTemplate : Entity<CycleTemplateId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Goal { get; private set; }
    public int? DurationDays { get; private set; }
    public int Position { get; private set; }

    private CycleTemplate(CycleTemplateDetails details, int position)
    {
        this.Name = details.Name.Trim();
        this.Goal = details.Goal?.Trim();
        this.DurationDays = details.DurationDays;
        this.Position = position;
    }

    private CycleTemplate() { }

    internal static CycleTemplate Create(CycleTemplateDetails details, int position) =>
        new CycleTemplate(details, position) { Id = new CycleTemplateId(Guid.NewGuid()) };

    internal void Update(CycleTemplateDetails details)
    {
        this.Name = details.Name.Trim();
        this.Goal = details.Goal?.Trim();
        this.DurationDays = details.DurationDays;
    }
}
