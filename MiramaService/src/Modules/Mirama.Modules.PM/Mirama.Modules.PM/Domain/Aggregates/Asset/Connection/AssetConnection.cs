using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Asset.Connection;

public sealed class AssetConnection : Entity<AssetConnectionId>
{
    public AssetConnectionTarget TargetType { get; private set; }
    public Guid TargetId { get; private set; }
    public Guid AddedByMemberId { get; private set; }
    public DateTime AddedAt { get; private set; }

    private AssetConnection(AssetConnectionDetails details)
    {
        this.TargetType = details.TargetType;
        this.TargetId = details.TargetId;
        this.AddedByMemberId = details.AddedByMemberId;
        this.AddedAt = DateTime.UtcNow;
    }

    private AssetConnection() { }

    internal static AssetConnection Create(AssetConnectionDetails details) =>
        new AssetConnection(details) { Id = new AssetConnectionId(Guid.NewGuid()) };
}
