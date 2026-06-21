using Mirama.Modules.PM.Domain.Aggregates.Asset.Version;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;

public sealed record AssetFeedbackDetails(
    AssetVersionId VersionId,
    Guid AuthorMemberId,
    string Content,
    FeedbackAnnotation? Annotation = null);
