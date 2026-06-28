using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses;

public sealed record StatusResponse(
    Guid StatusId,
    string Name,
    string? Color,
    string Category,
    int Position,
    bool IsDefault,
    bool IsTerminal);

internal static class StatusMapper
{
    internal static StatusResponse ToResponse(StatusConfig status) =>
        new(status.Id.Value, status.Name, status.Color, status.Category.ToString(), status.Position, status.IsDefault, status.IsTerminal);
}
