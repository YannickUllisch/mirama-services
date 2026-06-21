using ErrorOr;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Cycle;

public sealed class Cycle : OrganizationAggregateRoot<CycleId>
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Goal { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public CycleStatus Status { get; private set; }
    public DateTime DateCreated { get; private set; }

    public List<Guid> TaskIds { get; private set; } = [];

    private Cycle(CycleDetails details)
    {
        this.ProjectId = details.ProjectId;
        this.Name = details.Name.Trim();
        this.Goal = details.Goal?.Trim();
        this.StartDate = details.StartDate;
        this.EndDate = details.EndDate;
        this.Status = CycleStatus.Planning;
        this.DateCreated = DateTime.UtcNow;
    }

    private Cycle() { }

    public static Cycle Create(CycleDetails details) =>
        new Cycle(details) { Id = new CycleId(Guid.NewGuid()) };

    public void Update(CycleDetails details)
    {
        this.Name = details.Name.Trim();
        this.Goal = details.Goal?.Trim();
        this.StartDate = details.StartDate;
        this.EndDate = details.EndDate;
    }

    public ErrorOr<Success> Start()
    {
        if (this.Status != CycleStatus.Planning)
            return Error.Validation("Cycle.Start.InvalidStatus", "Only a planning cycle can be started.");
        this.Status = CycleStatus.Active;
        return Result.Success;
    }

    public ErrorOr<Success> Complete()
    {
        if (this.Status != CycleStatus.Active)
            return Error.Validation("Cycle.Complete.InvalidStatus", "Only an active cycle can be completed.");
        this.Status = CycleStatus.Completed;
        return Result.Success;
    }

    public void Cancel()
    {
        this.Status = CycleStatus.Cancelled;
    }

    public ErrorOr<Success> AddTask(Guid taskId)
    {
        if (this.TaskIds.Contains(taskId))
            return Error.Conflict("Cycle.Task.Duplicate", "Task is already in this cycle.");
        this.TaskIds.Add(taskId);
        return Result.Success;
    }

    public ErrorOr<Deleted> RemoveTask(Guid taskId)
    {
        if (!this.TaskIds.Remove(taskId))
            return Error.NotFound("Cycle.Task.NotFound", "Task not found in this cycle.");
        return Result.Deleted;
    }
}
