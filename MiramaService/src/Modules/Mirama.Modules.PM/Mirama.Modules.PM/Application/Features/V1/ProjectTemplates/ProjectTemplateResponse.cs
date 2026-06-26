using Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates;
using Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates;
using Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates;

public sealed record ProjectTemplateResponse(
    Guid TemplateId,
    string Name,
    string? Description,
    string? Category,
    bool IsPublic,
    DateTime DateCreated,
    List<TaskTemplateResponse> TaskTemplates,
    List<MilestoneTemplateResponse> MilestoneTemplates,
    List<CycleTemplateResponse> CycleTemplates);

internal static class ProjectTemplateMapper
{
    internal static ProjectTemplateResponse ToResponse(ProjectTemplate template) =>
        new(
            template.Id.Value,
            template.Name,
            template.Description,
            template.Category,
            template.IsPublic,
            template.DateCreated,
            template.TaskTemplates.Select(TaskTemplateMapper.ToResponse).ToList(),
            template.MilestoneTemplates.Select(MilestoneTemplateMapper.ToResponse).ToList(),
            template.CycleTemplates.Select(CycleTemplateMapper.ToResponse).ToList());
}
