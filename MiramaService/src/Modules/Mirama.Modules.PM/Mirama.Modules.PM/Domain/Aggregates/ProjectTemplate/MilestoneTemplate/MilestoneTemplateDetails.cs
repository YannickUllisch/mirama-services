namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;

public sealed record MilestoneTemplateDetails(
    string Title,
    int DayOffset,
    string? Description = null,
    string? Color = null);
