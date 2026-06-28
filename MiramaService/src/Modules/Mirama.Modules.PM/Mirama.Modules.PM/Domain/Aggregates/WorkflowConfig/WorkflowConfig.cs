using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;

public sealed class WorkflowConfig : OrganizationAggregateRoot<WorkflowConfigId>
{
    public Guid ProjectId { get; private set; }
    public List<StatusConfig> Statuses { get; private set; } = [];
    public List<PriorityConfig> Priorities { get; private set; } = [];

    private WorkflowConfig() { }

    public static WorkflowConfig CreateWithDefaults(Guid projectId)
    {
        var config = new WorkflowConfig { Id = new WorkflowConfigId(Guid.NewGuid()), ProjectId = projectId };
        config.SeedDefaultStatuses();
        config.SeedDefaultPriorities();
        return config;
    }

    public void SetProjectId(Guid projectId)
    {
        this.ProjectId = projectId;
    }

    // --- Status ---

    public ErrorOr<StatusConfig> AddStatus(StatusDetails details)
    {
        if (this.Statuses.Any(s => s.Name.Equals(details.Name, StringComparison.OrdinalIgnoreCase)))
            return Error.Conflict("WorkflowConfig.Status.Duplicate", "A status with this name already exists.");

        if (details.IsDefault)
            this.Statuses.ForEach(s => s.SetDefault(false));

        var status = StatusConfig.Create(details, this.Statuses.Count);
        this.Statuses.Add(status);
        return status;
    }

    public ErrorOr<Success> UpdateStatus(StatusConfigId id, StatusDetails details)
    {
        var status = this.Statuses.Find(s => s.Id == id);
        if (status is null)
            return Error.NotFound("WorkflowConfig.Status.NotFound", "Status not found.");

        if (this.Statuses.Any(s => s.Id != id && s.Name.Equals(details.Name, StringComparison.OrdinalIgnoreCase)))
            return Error.Conflict("WorkflowConfig.Status.Duplicate", "A status with this name already exists.");

        status.Update(details);
        return Result.Success;
    }

    public ErrorOr<Deleted> RemoveStatus(StatusConfigId id)
    {
        var status = this.Statuses.Find(s => s.Id == id);
        if (status is null)
            return Error.NotFound("WorkflowConfig.Status.NotFound", "Status not found.");
        if (status.IsDefault)
            return Error.Validation("WorkflowConfig.Status.DefaultRemoval", "Cannot remove the default status. Set another status as default first.");

        this.Statuses.Remove(status);
        return Result.Deleted;
    }

    public ErrorOr<Success> SetDefaultStatus(StatusConfigId id)
    {
        var status = this.Statuses.Find(s => s.Id == id);
        if (status is null)
            return Error.NotFound("WorkflowConfig.Status.NotFound", "Status not found.");
        if (status.IsTerminal)
            return Error.Validation("WorkflowConfig.Status.TerminalDefault", "A terminal status cannot be the default.");

        this.Statuses.ForEach(s => s.SetDefault(false));
        status.SetDefault(true);
        return Result.Success;
    }

    public ErrorOr<Success> ReorderStatuses(IReadOnlyList<StatusConfigId> orderedIds)
    {
        if (orderedIds.Count != this.Statuses.Count)
            return Error.Validation("WorkflowConfig.Status.Reorder", "Ordered list must include every status exactly once.");

        for (var i = 0; i < orderedIds.Count; i++)
        {
            var status = this.Statuses.Find(s => s.Id == orderedIds[i]);
            if (status is null)
                return Error.NotFound("WorkflowConfig.Status.NotFound", $"Status {orderedIds[i].Value} not found.");
            status.SetPosition(i);
        }
        return Result.Success;
    }

    // --- Priority ---

    public ErrorOr<PriorityConfig> AddPriority(PriorityDetails details)
    {
        if (this.Priorities.Any(p => p.Name.Equals(details.Name, StringComparison.OrdinalIgnoreCase)))
            return Error.Conflict("WorkflowConfig.Priority.Duplicate", "A priority with this name already exists.");

        if (details.IsDefault)
            this.Priorities.ForEach(p => p.SetDefault(false));

        var priority = PriorityConfig.Create(details);
        this.Priorities.Add(priority);
        return priority;
    }

    public ErrorOr<Success> UpdatePriority(PriorityConfigId id, PriorityDetails details)
    {
        var priority = this.Priorities.Find(p => p.Id == id);
        if (priority is null)
            return Error.NotFound("WorkflowConfig.Priority.NotFound", "Priority not found.");

        if (this.Priorities.Any(p => p.Id != id && p.Name.Equals(details.Name, StringComparison.OrdinalIgnoreCase)))
            return Error.Conflict("WorkflowConfig.Priority.Duplicate", "A priority with this name already exists.");

        priority.Update(details);
        return Result.Success;
    }

    public ErrorOr<Deleted> RemovePriority(PriorityConfigId id)
    {
        var priority = this.Priorities.Find(p => p.Id == id);
        if (priority is null)
            return Error.NotFound("WorkflowConfig.Priority.NotFound", "Priority not found.");
        if (priority.IsDefault)
            return Error.Validation("WorkflowConfig.Priority.DefaultRemoval", "Cannot remove the default priority. Set another priority as default first.");

        this.Priorities.Remove(priority);
        return Result.Deleted;
    }

    public ErrorOr<Success> SetDefaultPriority(PriorityConfigId id)
    {
        var priority = this.Priorities.Find(p => p.Id == id);
        if (priority is null)
            return Error.NotFound("WorkflowConfig.Priority.NotFound", "Priority not found.");

        this.Priorities.ForEach(p => p.SetDefault(false));
        priority.SetDefault(true);
        return Result.Success;
    }

    public ErrorOr<Success> ReorderPriorities(IReadOnlyList<PriorityConfigId> orderedIds)
    {
        if (orderedIds.Count != this.Priorities.Count)
            return Error.Validation("WorkflowConfig.Priority.Reorder", "Ordered list must include every priority exactly once.");

        for (var i = 0; i < orderedIds.Count; i++)
        {
            var priority = this.Priorities.Find(p => p.Id == orderedIds[i]);
            if (priority is null)
                return Error.NotFound("WorkflowConfig.Priority.NotFound", $"Priority {orderedIds[i].Value} not found.");
            priority.SetLevel(i);
        }
        return Result.Success;
    }

    // --- Seeding ---

    private void SeedDefaultStatuses()
    {
        var defaults = new StatusDetails[]
        {
            new("Backlog",     StatusCategory.NotStarted, "#94a3b8", IsDefault: false, IsTerminal: false),
            new("Todo",        StatusCategory.NotStarted, "#64748b", IsDefault: true,  IsTerminal: false),
            new("In Progress", StatusCategory.Active,     "#3b82f6", IsDefault: false, IsTerminal: false),
            new("In Review",   StatusCategory.Active,     "#8b5cf6", IsDefault: false, IsTerminal: false),
            new("Done",        StatusCategory.Done,       "#22c55e", IsDefault: false, IsTerminal: true),
            new("Cancelled",   StatusCategory.Cancelled,  "#ef4444", IsDefault: false, IsTerminal: true),
        };
        for (var i = 0; i < defaults.Length; i++)
            this.Statuses.Add(StatusConfig.Create(defaults[i], i));
    }

    private void SeedDefaultPriorities()
    {
        var defaults = new PriorityDetails[]
        {
            new("Low",      Level: 0, Color: "#94a3b8", IsDefault: true),
            new("Medium",   Level: 1, Color: "#f59e0b"),
            new("High",     Level: 2, Color: "#f97316"),
            new("Critical", Level: 3, Color: "#ef4444"),
        };
        foreach (var d in defaults)
            this.Priorities.Add(PriorityConfig.Create(d));
    }
}
