namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Connection;

public sealed record AssetConnectionDetails(
    AssetConnectionTarget TargetType,
    Guid TargetId,
    Guid AddedByMemberId);
