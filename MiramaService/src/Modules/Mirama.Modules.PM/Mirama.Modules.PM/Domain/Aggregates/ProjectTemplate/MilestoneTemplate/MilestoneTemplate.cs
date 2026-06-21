using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;

public sealed class MilestoneTemplate : Entity<MilestoneTemplateId>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int DayOffset { get; private set; }
    public string? Color { get; private set; }

    private MilestoneTemplate(MilestoneTemplateDetails details)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.DayOffset = details.DayOffset;
        this.Color = details.Color?.Trim();
    }

    private MilestoneTemplate() { }

    internal static MilestoneTemplate Create(MilestoneTemplateDetails details) =>
        new MilestoneTemplate(details) { Id = new MilestoneTemplateId(Guid.NewGuid()) };

    internal void Update(MilestoneTemplateDetails details)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.DayOffset = details.DayOffset;
        this.Color = details.Color?.Trim();
    }
}
