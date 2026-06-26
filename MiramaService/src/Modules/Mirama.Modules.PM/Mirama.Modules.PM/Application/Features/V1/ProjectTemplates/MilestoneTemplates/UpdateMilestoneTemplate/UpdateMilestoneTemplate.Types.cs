using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.UpdateMilestoneTemplate;

public sealed record UpdateMilestoneTemplateCommand(
    Guid ProjectTemplateId,
    Guid MilestoneTemplateId,
    string Title,
    int DayOffset,
    string? Description,
    string? Color) : ICommand<ErrorOr<MilestoneTemplateResponse>>;
