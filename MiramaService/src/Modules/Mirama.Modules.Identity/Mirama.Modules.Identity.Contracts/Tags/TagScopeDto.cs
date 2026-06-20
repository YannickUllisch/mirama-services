namespace Mirama.Modules.Identity.Contracts.Tags;

[Flags]
public enum TagScopeDto
{
    None = 0,
    General = 1,
    Project = 2,
    Task = 4,
    Asset = 8,
    Version = 16,
    Client = 32
}
