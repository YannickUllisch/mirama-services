
using ProjectService.Domain.ValueObjects;

namespace ProjectService.Domain.Entities;

public class Milestone : BaseEntity<MilestoneId>
{
    public Milestone(MilestoneId id, string title, DateOnly endDate, string color)
    {
        Id = id;
        Title = title;
        EndDate = endDate;
        Color = color;
    }
    
    public string Title { get; private set; } = string.Empty;

    public string Color { get; private set; } = string.Empty; 

    public DateOnly EndDate { get; private set; }
}