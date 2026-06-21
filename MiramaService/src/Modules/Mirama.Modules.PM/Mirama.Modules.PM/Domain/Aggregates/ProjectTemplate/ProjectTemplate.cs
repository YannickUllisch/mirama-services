using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;

public sealed class ProjectTemplate : OrganizationAggregateRoot<ProjectTemplateId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Category { get; private set; }
    public bool IsPublic { get; private set; }
    public DateTime DateCreated { get; private set; }

    public List<TaskTemplate.TaskTemplate> TaskTemplates { get; private set; } = [];
    public List<MilestoneTemplate.MilestoneTemplate> MilestoneTemplates { get; private set; } = [];
    public List<CycleTemplate.CycleTemplate> CycleTemplates { get; private set; } = [];

    private ProjectTemplate(ProjectTemplateDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.Category = details.Category?.Trim();
        this.IsPublic = false;
        this.DateCreated = DateTime.UtcNow;
    }

    private ProjectTemplate() { }

    public static ProjectTemplate Create(ProjectTemplateDetails details) =>
        new ProjectTemplate(details) { Id = new ProjectTemplateId(Guid.NewGuid()) };

    public void Update(ProjectTemplateDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.Category = details.Category?.Trim();
    }

    public void Publish() => this.IsPublic = true;
    public void Unpublish() => this.IsPublic = false;

    // --- Task templates ---

    public TaskTemplate.TaskTemplate AddTaskTemplate(TaskTemplateDetails details)
    {
        var task = TaskTemplate.TaskTemplate.Create(details, this.TaskTemplates.Count);
        this.TaskTemplates.Add(task);
        return task;
    }

    public ErrorOr<Deleted> RemoveTaskTemplate(TaskTemplateId id)
    {
        var task = this.TaskTemplates.Find(t => t.Id == id);
        if (task is null)
            return Error.NotFound("ProjectTemplate.Task.NotFound", "Task template not found.");

        this.TaskTemplates
            .Where(t => t.ParentTemplateTaskId == id)
            .ToList()
            .ForEach(t => t.ClearParent());

        this.TaskTemplates.Remove(task);
        return Result.Deleted;
    }

    // --- Milestone templates ---

    public MilestoneTemplate.MilestoneTemplate AddMilestoneTemplate(MilestoneTemplateDetails details)
    {
        var milestone = MilestoneTemplate.MilestoneTemplate.Create(details);
        this.MilestoneTemplates.Add(milestone);
        return milestone;
    }

    public ErrorOr<Deleted> RemoveMilestoneTemplate(MilestoneTemplateId id)
    {
        var milestone = this.MilestoneTemplates.Find(m => m.Id == id);
        if (milestone is null)
            return Error.NotFound("ProjectTemplate.Milestone.NotFound", "Milestone template not found.");
        this.MilestoneTemplates.Remove(milestone);
        return Result.Deleted;
    }

    // --- Cycle templates ---

    public CycleTemplate.CycleTemplate AddCycleTemplate(CycleTemplateDetails details)
    {
        var cycle = CycleTemplate.CycleTemplate.Create(details, this.CycleTemplates.Count);
        this.CycleTemplates.Add(cycle);
        return cycle;
    }

    public ErrorOr<Deleted> RemoveCycleTemplate(CycleTemplateId id)
    {
        var cycle = this.CycleTemplates.Find(c => c.Id == id);
        if (cycle is null)
            return Error.NotFound("ProjectTemplate.Cycle.NotFound", "Cycle template not found.");
        this.CycleTemplates.Remove(cycle);
        return Result.Deleted;
    }
}
