using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Task.Dependency;

public sealed class TaskDependency : Entity<TaskDependencyId>
{
    public TaskId BlockingTaskId { get; private set; } = default!;
    public DependencyType Type { get; private set; }

    private TaskDependency(TaskDependencyDetails details)
    {
        this.BlockingTaskId = details.BlockingTaskId;
        this.Type = details.Type;
    }

    private TaskDependency() { }

    internal static TaskDependency Create(TaskDependencyDetails details) =>
        new TaskDependency(details) { Id = new TaskDependencyId(Guid.NewGuid()) };
}
