using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;

public sealed class FeedbackReply : Entity<FeedbackReplyId>
{
    public Guid AuthorMemberId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastEditedAt { get; private set; }

    private FeedbackReply(FeedbackReplyDetails details)
    {
        this.AuthorMemberId = details.AuthorMemberId;
        this.Content = details.Content.Trim();
        this.CreatedAt = DateTime.UtcNow;
    }

    private FeedbackReply() { }

    internal static FeedbackReply Create(FeedbackReplyDetails details) =>
        new FeedbackReply(details) { Id = new FeedbackReplyId(Guid.NewGuid()) };

    internal void Edit(string content)
    {
        this.Content = content.Trim();
        this.LastEditedAt = DateTime.UtcNow;
    }
}
