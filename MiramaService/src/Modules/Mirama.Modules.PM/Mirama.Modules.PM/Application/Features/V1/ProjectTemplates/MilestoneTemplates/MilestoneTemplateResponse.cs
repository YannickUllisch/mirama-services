using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates;

public sealed record MilestoneTemplateResponse(
    Guid MilestoneTemplateId,
    string Title,
    string? Description,
    int DayOffset,
    string? Color);

internal static class MilestoneTemplateMapper
{
    internal static MilestoneTemplateResponse ToResponse(MilestoneTemplate milestone) =>
        new(milestone.Id.Value, milestone.Title, milestone.Description, milestone.DayOffset, milestone.Color);
}
