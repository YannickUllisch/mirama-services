using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;

public sealed class PriorityConfig : Entity<PriorityConfigId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Color { get; private set; }
    public string? Icon { get; private set; }
    public int Level { get; private set; }
    public bool IsDefault { get; private set; }

    private PriorityConfig(PriorityDetails details)
    {
        this.Name = details.Name.Trim();
        this.Color = details.Color?.Trim();
        this.Icon = details.Icon?.Trim();
        this.Level = details.Level;
        this.IsDefault = details.IsDefault;
    }

    private PriorityConfig() { }

    internal static PriorityConfig Create(PriorityDetails details) =>
        new PriorityConfig(details) { Id = new PriorityConfigId(Guid.NewGuid()) };

    internal void Update(PriorityDetails details)
    {
        this.Name = details.Name.Trim();
        this.Color = details.Color?.Trim();
        this.Icon = details.Icon?.Trim();
        this.Level = details.Level;
    }

    internal void SetDefault(bool isDefault) => this.IsDefault = isDefault;
    internal void SetLevel(int level) => this.Level = level;
}
