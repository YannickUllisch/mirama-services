namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;

public sealed record ProjectTemplateDetails(
    string Name,
    string? Description = null,
    string? Category = null);
