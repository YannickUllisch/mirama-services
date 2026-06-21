namespace Mirama.Modules.PM.Domain.Aggregates.Task;

public enum TaskType
{
    Epic,    // container — holds Story, Feature, Task, Issue, Test
    Story,   // container — holds Task, Issue
    Feature, // container — holds Task, Issue
    Task,    // leaf
    Issue,   // leaf
    Test     // leaf
}
