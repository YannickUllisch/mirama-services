namespace Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

public sealed record StatusDetails(
    string Name,
    StatusCategory Category,
    string? Color = null,
    bool IsDefault = false,
    bool IsTerminal = false);
