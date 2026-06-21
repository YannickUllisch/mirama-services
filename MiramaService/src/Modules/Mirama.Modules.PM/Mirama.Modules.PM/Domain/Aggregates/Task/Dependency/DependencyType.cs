namespace Mirama.Modules.PM.Domain.Aggregates.Task.Dependency;

public enum DependencyType
{
    FinishToStart,   // B cannot start until A finishes (classic "blocks")
    StartToStart,    // B cannot start until A starts
    FinishToFinish,  // B cannot finish until A finishes
    StartToFinish    // B cannot finish until A starts
}
