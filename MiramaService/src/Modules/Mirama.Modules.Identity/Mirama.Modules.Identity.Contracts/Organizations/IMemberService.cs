using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Contracts.Organizations;

public interface IMemberService : IModuleService
{
    Task<IReadOnlyList<MemberDto>> GetMembersAsync(Guid organizationId, CancellationToken ct = default);
    Task<MemberDto?> GetMemberByUserIdAsync(Guid organizationId, Guid userId, CancellationToken ct = default);
}
