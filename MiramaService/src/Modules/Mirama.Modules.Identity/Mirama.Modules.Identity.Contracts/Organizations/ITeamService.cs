using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Contracts.Organizations;

public interface ITeamService : IModuleService
{
    Task<IReadOnlyList<TeamDto>> GetTeamsByIdsAsync(IEnumerable<Guid> teamIds, CancellationToken ct = default);
    Task<TeamDto?> GetTeamByIdAsync(Guid teamId, CancellationToken ct = default);
}
