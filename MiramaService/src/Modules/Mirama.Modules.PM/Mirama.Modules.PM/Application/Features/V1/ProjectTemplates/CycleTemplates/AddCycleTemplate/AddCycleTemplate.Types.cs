using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.AddCycleTemplate;

public sealed record AddCycleTemplateCommand(
    Guid ProjectTemplateId,
    string Name,
    string? Goal,
    int? DurationDays) : ICommand<ErrorOr<CycleTemplateResponse>>;
