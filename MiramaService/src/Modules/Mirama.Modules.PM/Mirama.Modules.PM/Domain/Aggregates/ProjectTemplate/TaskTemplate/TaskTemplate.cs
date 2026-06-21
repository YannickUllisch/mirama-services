using Mirama.Modules.PM.Domain.Aggregates.Task;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;

public sealed class TaskTemplate : Entity<TaskTemplateId>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskType Type { get; private set; }
    public int? EstimatedHours { get; private set; }
    public TaskTemplateId? ParentTemplateTaskId { get; private set; }
    public int Position { get; private set; }

    private TaskTemplate(TaskTemplateDetails details, int position)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.Type = details.Type;
        this.EstimatedHours = details.EstimatedHours;
        this.ParentTemplateTaskId = details.ParentTemplateTaskId;
        this.Position = position;
    }

    private TaskTemplate() { }

    internal static TaskTemplate Create(TaskTemplateDetails details, int position) =>
        new TaskTemplate(details, position) { Id = new TaskTemplateId(Guid.NewGuid()) };

    internal void ClearParent() => this.ParentTemplateTaskId = null;
}
