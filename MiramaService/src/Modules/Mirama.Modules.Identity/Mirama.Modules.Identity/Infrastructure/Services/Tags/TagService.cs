using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Tags;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Tag;
using Mirama.Modules.Identity.Infrastructure.Persistence;

namespace Mirama.Modules.Identity.Infrastructure.Services.Tags;

internal sealed class TagService(IdentityDbContext db) : ITagService
{
    public async Task<IReadOnlyList<TagDto>> GetTagsAsync(
        Guid organizationId, TagScopeDto? scope = null, CancellationToken ct = default)
    {
        var query = db.Tags.AsNoTracking()
            .Where(t => t.OrganizationId == organizationId);

        if (scope.HasValue)
        {
            var domainScope = (TagScope)(int)scope.Value;
            query = query.Where(t => (t.Scope & domainScope) != 0 || (t.Scope & TagScope.General) != 0);
        }

        var tags = await query.ToListAsync(ct);

        return tags.Select(t => new TagDto(
            t.Id.Value,
            t.OrganizationId,
            t.Name,
            t.Slug,
            t.Color,
            t.Description,
            (TagScopeDto)(int)t.Scope,
            t.DateCreated)).ToList();
    }

    public async Task<TagDto?> GetTagByIdAsync(
        Guid organizationId, Guid tagId, CancellationToken ct = default)
    {
        var tag = await db.Tags.AsNoTracking()
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.Id == new TagId(tagId), ct);

        return tag is null ? null : new TagDto(
            tag.Id.Value,
            tag.OrganizationId,
            tag.Name,
            tag.Slug,
            tag.Color,
            tag.Description,
            (TagScopeDto)(int)tag.Scope,
            tag.DateCreated);
    }
}
