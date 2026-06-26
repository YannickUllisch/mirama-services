using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.Asset.Connection;
using Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;
using Mirama.Modules.PM.Domain.Aggregates.Asset.Version;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset;

public sealed class Asset : OrganizationAggregateRoot<AssetId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? ProjectId { get; private set; }
    public AssetType Type { get; private set; }
    public AssetStatus Status { get; private set; }
    public AssetVersionId? CurrentVersionId { get; private set; }
    public Guid CreatedByMemberId { get; private set; }
    public DateTime DateCreated { get; private set; }

    public List<Guid> TagIds { get; private set; } = [];
    public List<AssetVersion> Versions { get; private set; } = [];
    public List<AssetFeedback> Feedbacks { get; private set; } = [];
    public List<AssetConnection> Connections { get; private set; } = [];

    private Asset(AssetDetails details)
    {
        this.Name = details.Name.Trim();
        this.Description = details.Description?.Trim();
        this.ProjectId = details.ProjectId;
        this.Type = details.Type;
        this.Status = AssetStatus.Draft;
        this.CreatedByMemberId = details.CreatedByMemberId;
        this.DateCreated = DateTime.UtcNow;
    }

    private Asset() { }

    public static Asset Create(AssetDetails details) =>
        new Asset(details) { Id = new AssetId(Guid.NewGuid()) };

    public void Update(string name, string? description)
    {
        this.Name = name.Trim();
        this.Description = description?.Trim();
    }

    // --- Approval flow ---

    public ErrorOr<Success> SubmitForReview()
    {
        if (this.Status != AssetStatus.Draft && this.Status != AssetStatus.ChangesRequested)
            return Error.Validation("Asset.SubmitForReview.InvalidStatus", "Only draft or changes-requested assets can be submitted for review.");
        if (this.CurrentVersionId is null)
            return Error.Validation("Asset.SubmitForReview.NoVersion", "Asset must have at least one version before review.");
        this.Status = AssetStatus.InReview;
        return Result.Success;
    }

    public ErrorOr<Success> Approve(Guid memberId)
    {
        if (this.Status != AssetStatus.InReview)
            return Error.Validation("Asset.Approve.InvalidStatus", "Only assets in review can be approved.");
        this.Status = AssetStatus.Approved;
        return Result.Success;
    }

    public ErrorOr<Success> RequestChanges(Guid memberId)
    {
        if (this.Status != AssetStatus.InReview)
            return Error.Validation("Asset.RequestChanges.InvalidStatus", "Only assets in review can have changes requested.");
        this.Status = AssetStatus.ChangesRequested;
        return Result.Success;
    }

    public ErrorOr<Success> Reject(Guid memberId)
    {
        if (this.Status != AssetStatus.InReview)
            return Error.Validation("Asset.Reject.InvalidStatus", "Only assets in review can be rejected.");
        this.Status = AssetStatus.Rejected;
        return Result.Success;
    }

    public void Archive() => this.Status = AssetStatus.Archived;

    public AssetVersion AddVersion(AssetVersionDetails details)
    {
        var version = AssetVersion.Create(details, this.Versions.Count + 1);
        this.Versions.Add(version);
        this.CurrentVersionId = version.Id;
        return version;
    }

    public ErrorOr<Success> SetCurrentVersion(AssetVersionId versionId)
    {
        if (!this.Versions.Any(v => v.Id == versionId))
            return Error.NotFound("Asset.Version.NotFound", "Version not found on this asset.");
        this.CurrentVersionId = versionId;
        return Result.Success;
    }

    public ErrorOr<AssetFeedback> AddFeedback(AssetFeedbackDetails details)
    {
        if (!this.Versions.Any(v => v.Id == details.VersionId))
            return Error.NotFound("Asset.Feedback.VersionNotFound", "Target version not found on this asset.");
        var feedback = AssetFeedback.Create(details);
        this.Feedbacks.Add(feedback);
        return feedback;
    }

    public ErrorOr<Success> ResolveFeedback(AssetFeedbackId feedbackId, Guid memberId)
    {
        var feedback = this.Feedbacks.Find(f => f.Id == feedbackId);
        if (feedback is null)
            return Error.NotFound("Asset.Feedback.NotFound", "Feedback not found.");
        return feedback.Resolve(memberId);
    }

    public ErrorOr<Success> MarkFeedbackWontFix(AssetFeedbackId feedbackId, Guid memberId)
    {
        var feedback = this.Feedbacks.Find(f => f.Id == feedbackId);
        if (feedback is null)
            return Error.NotFound("Asset.Feedback.NotFound", "Feedback not found.");
        return feedback.MarkWontFix(memberId);
    }

    public ErrorOr<FeedbackReply> AddFeedbackReply(AssetFeedbackId feedbackId, FeedbackReplyDetails details)
    {
        var feedback = this.Feedbacks.Find(f => f.Id == feedbackId);
        if (feedback is null)
            return Error.NotFound("Asset.Feedback.NotFound", "Feedback not found.");
        return feedback.AddReply(details);
    }

    public ErrorOr<Deleted> RemoveFeedbackReply(AssetFeedbackId feedbackId, FeedbackReplyId replyId, Guid requestingMemberId)
    {
        var feedback = this.Feedbacks.Find(f => f.Id == feedbackId);
        if (feedback is null)
            return Error.NotFound("Asset.Feedback.NotFound", "Feedback not found.");
        return feedback.RemoveReply(replyId, requestingMemberId);
    }

    public ErrorOr<AssetConnection> AddConnection(AssetConnectionDetails details)
    {
        if (this.Connections.Any(c => c.TargetType == details.TargetType && c.TargetId == details.TargetId))
            return Error.Conflict("Asset.Connection.Duplicate", "This connection already exists.");
        var connection = AssetConnection.Create(details);
        this.Connections.Add(connection);
        return connection;
    }

    public ErrorOr<Deleted> RemoveConnection(AssetConnectionId connectionId)
    {
        var connection = this.Connections.Find(c => c.Id == connectionId);
        if (connection is null)
            return Error.NotFound("Asset.Connection.NotFound", "Connection not found.");
        this.Connections.Remove(connection);
        return Result.Deleted;
    }

    public ErrorOr<Success> AddTag(Guid tagId)
    {
        if (this.TagIds.Contains(tagId))
            return Error.Conflict("Asset.Tag.Duplicate", "Tag already applied.");
        this.TagIds.Add(tagId);
        return Result.Success;
    }

    public void RemoveTag(Guid tagId) => this.TagIds.Remove(tagId);
}
