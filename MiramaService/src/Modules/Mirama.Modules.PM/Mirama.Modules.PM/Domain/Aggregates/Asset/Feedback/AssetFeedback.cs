using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.Asset.Version;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;

public sealed class AssetFeedback : Entity<AssetFeedbackId>
{
    public AssetVersionId VersionId { get; private set; } = default!;
    public Guid AuthorMemberId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public AssetFeedbackStatus Status { get; private set; }
    public FeedbackAnnotation? Annotation { get; private set; }
    public Guid? ResolvedByMemberId { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public List<FeedbackReply> Replies { get; private set; } = [];

    private AssetFeedback(AssetFeedbackDetails details)
    {
        this.VersionId = details.VersionId;
        this.AuthorMemberId = details.AuthorMemberId;
        this.Content = details.Content.Trim();
        this.Status = AssetFeedbackStatus.Open;
        this.Annotation = details.Annotation;
        this.CreatedAt = DateTime.UtcNow;
    }

    private AssetFeedback() { }

    internal static AssetFeedback Create(AssetFeedbackDetails details) =>
        new AssetFeedback(details) { Id = new AssetFeedbackId(Guid.NewGuid()) };

    internal ErrorOr<Success> Resolve(Guid memberId)
    {
        if (this.Status != AssetFeedbackStatus.Open)
            return Error.Validation("AssetFeedback.Resolve.InvalidStatus", "Only open feedback can be resolved.");
        this.Status = AssetFeedbackStatus.Resolved;
        this.ResolvedByMemberId = memberId;
        this.ResolvedAt = DateTime.UtcNow;
        return Result.Success;
    }

    internal ErrorOr<Success> MarkWontFix(Guid memberId)
    {
        if (this.Status != AssetFeedbackStatus.Open)
            return Error.Validation("AssetFeedback.WontFix.InvalidStatus", "Only open feedback can be marked won't fix.");
        this.Status = AssetFeedbackStatus.WontFix;
        this.ResolvedByMemberId = memberId;
        this.ResolvedAt = DateTime.UtcNow;
        return Result.Success;
    }

    internal FeedbackReply AddReply(FeedbackReplyDetails details)
    {
        var reply = FeedbackReply.Create(details);
        this.Replies.Add(reply);
        return reply;
    }

    internal ErrorOr<Deleted> RemoveReply(FeedbackReplyId replyId, Guid requestingMemberId)
    {
        var reply = this.Replies.Find(r => r.Id == replyId);
        if (reply is null)
            return Error.NotFound("AssetFeedback.Reply.NotFound", "Reply not found.");
        if (reply.AuthorMemberId != requestingMemberId)
            return Error.Forbidden("AssetFeedback.Reply.Forbidden", "Only the reply author can remove it.");
        this.Replies.Remove(reply);
        return Result.Deleted;
    }
}
