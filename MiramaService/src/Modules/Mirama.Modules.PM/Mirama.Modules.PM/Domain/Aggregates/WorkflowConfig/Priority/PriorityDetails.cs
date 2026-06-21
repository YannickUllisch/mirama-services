namespace Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;

public sealed record PriorityDetails(
    string Name,
    int Level,
    string? Color = null,
    string? Icon = null,
    bool IsDefault = false);
