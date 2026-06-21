namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;

public sealed record FeedbackReplyDetails(
    string Content,
    Guid AuthorMemberId);
