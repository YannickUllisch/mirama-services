using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.UpdateCycleTemplate;

public sealed record UpdateCycleTemplateCommand(
    Guid ProjectTemplateId,
    Guid CycleTemplateId,
    string Name,
    string? Goal,
    int? DurationDays) : ICommand<ErrorOr<CycleTemplateResponse>>;
