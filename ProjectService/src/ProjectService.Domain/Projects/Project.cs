

using ProjectService.Domain.Enums;
using ProjectService.Domain.ValueObjects;

namespace ProjectService.Domain.Entities;

public class Project : BaseEntity<ProjectId>
{
    public static Project Create(
        string name,
        string desc,
        DateOnly startDate,
        DateOnly endDate,
        StatusType status,
        PriorityType priority,
        int budget,
        bool archived,
        TeamId teamId)
    {
        // Validate invariants once
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (endDate < startDate) throw new ArgumentException("End date must be after start date.");
        if (budget < 0) throw new ArgumentOutOfRangeException(nameof(budget));

        var p = new Project
        {
            Id = new ProjectId(Guid.NewGuid()),
            Name = name.Trim(),
            Description = desc?.Trim() ?? string.Empty,
            StartDate = startDate,
            EndDate = endDate,
            Status = status,
            Priority = priority,
            Budget = budget,
            Archived = archived,
            TeamId = teamId
        };
        return p;
    }
    public string Name { get; private set; } = string.Empty;
    
    public string Description { get; private set; } = string.Empty;

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public StatusType Status { get; private set; }

    public PriorityType Priority { get; private set; }

    public int Budget { get; private set; } 

    public bool Archived { get; private set; }

    public TeamId TeamId { get; private set; } = default!;

    public ICollection<TaskId> TaskIds { get; private set; } = [];

    public ICollection<TagId> TagIds { get; private set; } = [];

    public ICollection<ProjectMember> Members { get; private set; } = [];
}