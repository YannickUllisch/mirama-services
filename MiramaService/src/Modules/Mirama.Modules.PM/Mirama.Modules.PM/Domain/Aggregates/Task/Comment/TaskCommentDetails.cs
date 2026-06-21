namespace Mirama.Modules.PM.Domain.Aggregates.Task.Comment;

public sealed record TaskCommentDetails(
    string Content,
    Guid AuthorMemberId,
    TaskId TaskId,
    TaskCommentId? ParentCommentId = null);
