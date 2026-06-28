using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Priorities;

public sealed record PriorityResponse(
    Guid PriorityId,
    string Name,
    string? Color,
    string? Icon,
    int Level,
    bool IsDefault);

internal static class PriorityMapper
{
    internal static PriorityResponse ToResponse(PriorityConfig priority) =>
        new(priority.Id.Value, priority.Name, priority.Color, priority.Icon, priority.Level, priority.IsDefault);
}
