namespace Mirama.Modules.PM.Domain.Aggregates.Asset;

public sealed record AssetDetails(
    string Name,
    AssetType Type,
    Guid CreatedByMemberId,
    Guid? ProjectId = null,
    string? Description = null);
