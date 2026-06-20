using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Contracts.Tags;

public interface ITagService : IModuleService
{
    Task<IReadOnlyList<TagDto>> GetTagsAsync(Guid organizationId, TagScopeDto? scope = null, CancellationToken ct = default);
    Task<TagDto?> GetTagByIdAsync(Guid organizationId, Guid tagId, CancellationToken ct = default);
}
