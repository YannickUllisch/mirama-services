using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;

public sealed class StatusConfig : Entity<StatusConfigId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Color { get; private set; }
    public StatusCategory Category { get; private set; }
    public int Position { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsTerminal { get; private set; }

    private StatusConfig(StatusDetails details, int position)
    {
        this.Name = details.Name.Trim();
        this.Color = details.Color?.Trim();
        this.Category = details.Category;
        this.Position = position;
        this.IsDefault = details.IsDefault;
        this.IsTerminal = details.IsTerminal;
    }

    private StatusConfig() { }

    internal static StatusConfig Create(StatusDetails details, int position) =>
        new StatusConfig(details, position) { Id = new StatusConfigId(Guid.NewGuid()) };

    internal void Update(StatusDetails details)
    {
        this.Name = details.Name.Trim();
        this.Color = details.Color?.Trim();
        this.Category = details.Category;
        this.IsTerminal = details.IsTerminal;
    }

    internal void SetDefault(bool isDefault) => this.IsDefault = isDefault;
    internal void SetPosition(int position) => this.Position = position;
}
