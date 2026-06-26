using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates;

public sealed record CycleTemplateResponse(
    Guid CycleTemplateId,
    string Name,
    string? Goal,
    int? DurationDays,
    int Position);

internal static class CycleTemplateMapper
{
    internal static CycleTemplateResponse ToResponse(CycleTemplate cycle) =>
        new(cycle.Id.Value, cycle.Name, cycle.Goal, cycle.DurationDays, cycle.Position);
}
