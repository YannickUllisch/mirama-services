using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;

namespace Mirama.Modules.Identity.Infrastructure.Services.Organizations;

internal sealed class MemberService(IdentityDbContext db) : IMemberService
{
    public async Task<IReadOnlyList<MemberDto>> GetMembersAsync(
        Guid organizationId, CancellationToken ct = default)
    {
        var members = await db.Members.AsNoTracking()
            .Where(m => m.OrganizationId == organizationId)
            .ToListAsync(ct);

        return [..members.Select(Map)];
    }

    public async Task<IReadOnlyList<MemberDto>> GetMembersByIdsAsync(
        IEnumerable<Guid> memberIds, CancellationToken ct = default)
    {
        var ids = memberIds.ToList();
        var members = await db.Members.AsNoTracking()
            .Where(m => ids.Contains(m.Id.Value))
            .ToListAsync(ct);

        return [..members.Select(Map)];
    }

    public async Task<MemberDto?> GetMemberByUserIdAsync(
        Guid organizationId, Guid userId, CancellationToken ct = default)
    {
        var member = await db.Members.AsNoTracking()
            .FirstOrDefaultAsync(m => m.OrganizationId == organizationId
                && m.UserId == new UserId(userId), ct);

        return member is null ? null : Map(member);
    }

    private static MemberDto Map(Member m) => new(
        m.Id.Value,
        m.OrganizationId,
        m.UserId.Value,
        m.Name,
        m.Email,
        [..m.IamRoleIds.Select(r => r.Value)]);
}
