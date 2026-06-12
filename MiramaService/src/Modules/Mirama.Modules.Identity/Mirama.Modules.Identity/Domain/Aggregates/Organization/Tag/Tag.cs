using System.Text.RegularExpressions;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;

public sealed class Tag : OrganizationAggregateRoot<TagId>
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Color { get; private set; }
    public string? Description { get; private set; }
    public TagScope Scope { get; private set; }
    public DateTime DateCreated { get; private set; }

    private Tag(TagDetails details)
    {
        this.Name = details.Name.Trim();
        this.Slug = GenerateSlug(details.Name);
        this.Color = details.Color?.Trim();
        this.Description = details.Description?.Trim();
        this.Scope = details.Scope;
        this.DateCreated = DateTime.UtcNow;
    }

    private Tag() { }

    public static Tag Create(TagDetails details) =>
        new Tag(details) { Id = new TagId(Guid.NewGuid()) };

    public void Update(TagDetails details)
    {
        this.Name = details.Name.Trim();
        this.Slug = GenerateSlug(details.Name);
        this.Color = details.Color?.Trim();
        this.Description = details.Description?.Trim();
        this.Scope = details.Scope;
    }

    public static string GenerateSlug(string name) =>
        Regex.Replace(name.Trim().ToLowerInvariant().Replace(' ', '-'), @"[^a-z0-9\-]", string.Empty);
}
