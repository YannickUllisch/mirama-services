using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Task.TimeLog;

public sealed class TimeLog : Entity<TimeLogId>
{
    public Guid MemberId { get; private set; }
    public int Minutes { get; private set; }
    public DateOnly WorkedOn { get; private set; }
    public string? Description { get; private set; }
    public DateTime LoggedAt { get; private set; }

    private TimeLog(TimeLogDetails details)
    {
        this.MemberId = details.MemberId;
        this.Minutes = details.Minutes;
        this.WorkedOn = details.WorkedOn;
        this.Description = details.Description?.Trim();
        this.LoggedAt = DateTime.UtcNow;
    }

    private TimeLog() { }

    internal static TimeLog Create(TimeLogDetails details) =>
        new TimeLog(details) { Id = new TimeLogId(Guid.NewGuid()) };

    internal void Update(TimeLogDetails details)
    {
        this.Minutes = details.Minutes;
        this.WorkedOn = details.WorkedOn;
        this.Description = details.Description?.Trim();
    }
}
