namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;

public sealed record CycleTemplateDetails(
    string Name,
    string? Goal = null,
    int? DurationDays = null);
