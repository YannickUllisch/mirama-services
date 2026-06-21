namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Feedback;

// Spatial: X/Y/Width/Height in 0.0–1.0 percentage of the asset dimensions (image, video frame).
// Temporal: StartSeconds/EndSeconds for video/audio annotations.
// Document: PageNumber for PDF/document annotations.
// Populate only the fields relevant to the asset type — the rest stay null.
public sealed record FeedbackAnnotation(
    float? X = null,
    float? Y = null,
    float? Width = null,
    float? Height = null,
    float? StartSeconds = null,
    float? EndSeconds = null,
    int? PageNumber = null);
