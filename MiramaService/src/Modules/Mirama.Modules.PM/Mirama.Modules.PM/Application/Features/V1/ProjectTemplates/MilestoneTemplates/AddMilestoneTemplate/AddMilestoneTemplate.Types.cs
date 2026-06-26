using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.AddMilestoneTemplate;

public sealed record AddMilestoneTemplateCommand(
    Guid ProjectTemplateId,
    string Title,
    int DayOffset,
    string? Description,
    string? Color) : ICommand<ErrorOr<MilestoneTemplateResponse>>;
