using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;

public sealed class ProjectMilestone : OrganizationEntity<ProjectMilestoneId>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public MilestoneStatus Status { get; private set; }
    public string? Color { get; private set; }
    public DateTime DateCreated { get; private set; }

    private ProjectMilestone(ProjectMilestoneDetails details)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.DueDate = details.DueDate;
        this.Status = MilestoneStatus.Pending;
        this.Color = details.Color?.Trim();
        this.DateCreated = DateTime.UtcNow;
    }

    private ProjectMilestone() { }

    internal static ProjectMilestone Create(ProjectMilestoneDetails details) =>
        new ProjectMilestone(details) { Id = new ProjectMilestoneId(Guid.NewGuid()) };

    public void Update(ProjectMilestoneDetails details)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.DueDate = details.DueDate;
        this.Color = details.Color?.Trim();
    }

    public void Complete()
    {
        this.Status = MilestoneStatus.Achieved;
    }

    public void Reopen()
    {
        this.Status = MilestoneStatus.Pending;
    }

    public void MarkMissed()
    {
        this.Status = MilestoneStatus.Missed;
    }
}
