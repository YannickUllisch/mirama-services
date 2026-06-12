namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;

public record TagDetails(string Name, string? Color, string? Description, TagScope Scope);
