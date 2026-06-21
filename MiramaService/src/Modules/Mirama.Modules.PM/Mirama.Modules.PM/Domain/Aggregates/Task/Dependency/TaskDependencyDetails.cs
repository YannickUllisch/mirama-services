namespace Mirama.Modules.PM.Domain.Aggregates.Task.Dependency;

public sealed record TaskDependencyDetails(
    TaskId BlockingTaskId,
    DependencyType Type = DependencyType.FinishToStart);
