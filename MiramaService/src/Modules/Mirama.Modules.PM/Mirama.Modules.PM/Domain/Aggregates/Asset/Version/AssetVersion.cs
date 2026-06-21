using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Version;

public sealed class AssetVersion : Entity<AssetVersionId>
{
    public int VersionNumber { get; private set; }
    public string? Label { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public string MimeType { get; private set; } = string.Empty;
    public Guid UploadedByMemberId { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string? Notes { get; private set; }

    private AssetVersion(AssetVersionDetails details, int versionNumber)
    {
        this.VersionNumber = versionNumber;
        this.Label = details.Label?.Trim();
        this.StorageKey = details.StorageKey.Trim();
        this.FileName = details.FileName.Trim();
        this.FileSizeBytes = details.FileSizeBytes;
        this.MimeType = details.MimeType.Trim();
        this.UploadedByMemberId = details.UploadedByMemberId;
        this.UploadedAt = DateTime.UtcNow;
        this.Notes = details.Notes?.Trim();
    }

    private AssetVersion() { }

    internal static AssetVersion Create(AssetVersionDetails details, int versionNumber) =>
        new AssetVersion(details, versionNumber) { Id = new AssetVersionId(Guid.NewGuid()) };
}
