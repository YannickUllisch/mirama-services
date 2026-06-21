using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.Task.Comment;
using Mirama.Modules.PM.Domain.Aggregates.Task.Dependency;
using Mirama.Modules.PM.Domain.Aggregates.Task.TimeLog;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Task;

public sealed class Task : OrganizationAggregateRoot<TaskId>
{
    public string TaskCode { get; private set; } = string.Empty;
    public TaskType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid StatusId { get; private set; }
    public Guid PriorityId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public int? EstimatedHours { get; private set; }
    public Guid? AssignedToMemberId { get; private set; }
    public TaskId? ParentTaskId { get; private set; }
    public DateTime DateCreated { get; private set; }

    public List<Guid> TagIds { get; private set; } = [];
    public List<TaskComment> Comments { get; private set; } = [];
    public List<TaskDependency> Dependencies { get; private set; } = [];
    public List<TimeLog.TimeLog> TimeLogs { get; private set; } = [];
    public List<Guid> WatcherMemberIds { get; private set; } = [];

    public Guid ProjectId { get; private set; }

    private Task(TaskDetails details)
    {
        this.TaskCode = details.TaskCode.Trim().ToUpperInvariant();
        this.Type = details.Type;
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.StatusId = details.StatusId;
        this.PriorityId = details.PriorityId;
        this.StartDate = details.StartDate ?? DateTime.UtcNow;
        this.DueDate = details.DueDate ?? DateTime.UtcNow.AddDays(7);
        this.EstimatedHours = details.EstimatedHours;
        this.AssignedToMemberId = details.AssignedToMemberId;
        this.ParentTaskId = details.ParentTaskId;
        this.ProjectId = details.ProjectId.Value;
        this.DateCreated = DateTime.UtcNow;
    }

    private Task() { }

    public static Task Create(TaskDetails details) =>
        new Task(details) { Id = new TaskId(Guid.NewGuid()) };

    public void Update(TaskDetails details)
    {
        this.Title = details.Title.Trim();
        this.Description = details.Description?.Trim();
        this.StartDate = details.StartDate ?? this.StartDate;
        this.DueDate = details.DueDate ?? this.DueDate;
        this.EstimatedHours = details.EstimatedHours;
    }

    public ErrorOr<Success> SetParent(TaskId parentId, TaskType parentType)
    {
        if (!CanBeParentOf(parentType, this.Type))
            return Error.Validation(
                "Task.Parent.InvalidHierarchy",
                $"A {parentType} cannot contain a {this.Type}.");

        this.ParentTaskId = parentId;
        return Result.Success;
    }

    public void ClearParent()
    {
        this.ParentTaskId = null;
    }

    public void Assign(Guid memberId)
    {
        this.AssignedToMemberId = memberId;
    }

    public void Unassign()
    {
        this.AssignedToMemberId = null;
    }

    public void SetStatus(Guid statusId)
    {
        this.StatusId = statusId;
    }

    public void SetPriority(Guid priorityId)
    {
        this.PriorityId = priorityId;
    }

    public TaskComment AddComment(TaskCommentDetails details)
    {
        var comment = TaskComment.Create(details);
        this.Comments.Add(comment);
        return comment;
    }

    public ErrorOr<Deleted> RemoveComment(TaskCommentId commentId, Guid requestingMemberId)
    {
        var comment = this.Comments.Find(c => c.Id == commentId);
        if (comment is null)
            return Error.NotFound("Task.Comment.NotFound", "Comment not found.");
        if (comment.AuthorMemberId != requestingMemberId)
            return Error.Forbidden("Task.Comment.Forbidden", "Only the comment author can remove it.");
        this.Comments.Remove(comment);
        return Result.Deleted;
    }

    public ErrorOr<Success> AddTag(Guid tagId)
    {
        if (this.TagIds.Contains(tagId))
            return Error.Conflict("Task.Tag.Duplicate", "Tag already applied.");
        this.TagIds.Add(tagId);
        return Result.Success;
    }

    public void RemoveTag(Guid tagId)
    {
        this.TagIds.Remove(tagId);
    }

    // --- Dependencies ---

    public ErrorOr<TaskDependency> AddDependency(TaskDependencyDetails details)
    {
        if (details.BlockingTaskId == this.Id)
            return Error.Validation("Task.Dependency.SelfReference", "A task cannot depend on itself.");
        if (this.Dependencies.Any(d => d.BlockingTaskId == details.BlockingTaskId && d.Type == details.Type))
            return Error.Conflict("Task.Dependency.Duplicate", "This dependency already exists.");
        var dependency = TaskDependency.Create(details);
        this.Dependencies.Add(dependency);
        return dependency;
    }

    public ErrorOr<Deleted> RemoveDependency(TaskDependencyId id)
    {
        var dependency = this.Dependencies.Find(d => d.Id == id);
        if (dependency is null)
            return Error.NotFound("Task.Dependency.NotFound", "Dependency not found.");
        this.Dependencies.Remove(dependency);
        return Result.Deleted;
    }

    // --- Time logs ---

    public TimeLog.TimeLog LogTime(TimeLogDetails details)
    {
        var log = TimeLog.TimeLog.Create(details);
        this.TimeLogs.Add(log);
        return log;
    }

    public ErrorOr<Success> UpdateTimeLog(TimeLogId id, TimeLogDetails details, Guid requestingMemberId)
    {
        var log = this.TimeLogs.Find(l => l.Id == id);
        if (log is null)
            return Error.NotFound("Task.TimeLog.NotFound", "Time log not found.");
        if (log.MemberId != requestingMemberId)
            return Error.Forbidden("Task.TimeLog.Forbidden", "Only the member who logged time can update it.");
        log.Update(details);
        return Result.Success;
    }

    public ErrorOr<Deleted> RemoveTimeLog(TimeLogId id, Guid requestingMemberId)
    {
        var log = this.TimeLogs.Find(l => l.Id == id);
        if (log is null)
            return Error.NotFound("Task.TimeLog.NotFound", "Time log not found.");
        if (log.MemberId != requestingMemberId)
            return Error.Forbidden("Task.TimeLog.Forbidden", "Only the member who logged time can remove it.");
        this.TimeLogs.Remove(log);
        return Result.Deleted;
    }

    // --- Watchers ---

    public ErrorOr<Success> Watch(Guid memberId)
    {
        if (this.WatcherMemberIds.Contains(memberId))
            return Error.Conflict("Task.Watcher.Duplicate", "Member is already watching this task.");
        this.WatcherMemberIds.Add(memberId);
        return Result.Success;
    }

    public void Unwatch(Guid memberId)
    {
        this.WatcherMemberIds.Remove(memberId);
    }

    public bool IsContainer() =>
        this.Type is TaskType.Epic or TaskType.Story or TaskType.Feature;

    public static bool CanBeParentOf(TaskType parent, TaskType child) =>
        parent switch
        {
            TaskType.Epic    => child is TaskType.Story or TaskType.Feature
                                        or TaskType.Task or TaskType.Issue or TaskType.Test,
            TaskType.Story   => child is TaskType.Task or TaskType.Issue,
            TaskType.Feature => child is TaskType.Task or TaskType.Issue,
            _                => false
        };
}
