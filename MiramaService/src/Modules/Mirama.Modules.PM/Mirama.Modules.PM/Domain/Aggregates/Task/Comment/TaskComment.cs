using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Task.Comment;

public sealed class TaskComment : Entity<TaskCommentId>
{
    public TaskId TaskId { get; private set; } = default!;
    public Guid AuthorMemberId { get; private set; }
    public TaskCommentId? ParentCommentId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime DateCreated { get; private set; }
    public DateTime? LastEditedAt { get; private set; }

    private TaskComment(TaskCommentDetails details)
    {
        this.TaskId = details.TaskId;
        this.AuthorMemberId = details.AuthorMemberId;
        this.ParentCommentId = details.ParentCommentId;
        this.Content = details.Content.Trim();
        this.DateCreated = DateTime.UtcNow;
    }

    private TaskComment() { }

    internal static TaskComment Create(TaskCommentDetails details) =>
        new TaskComment(details) { Id = new TaskCommentId(Guid.NewGuid()) };

    public void Edit(string newContent)
    {
        this.Content = newContent.Trim();
        this.LastEditedAt = DateTime.UtcNow;
    }
}
