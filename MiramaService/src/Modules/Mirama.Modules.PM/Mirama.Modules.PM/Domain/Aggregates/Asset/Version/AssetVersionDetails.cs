namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Version;

public sealed record AssetVersionDetails(
    string StorageKey,
    string FileName,
    long FileSizeBytes,
    string MimeType,
    Guid UploadedByMemberId,
    string? Label = null,
    string? Notes = null);
