using Mirama.SharedKernel.Abstractions.Permissions;

namespace Mirama.SharedKernel.Models.Permissions;

internal sealed class NullProjectRoleProvider : IProjectRoleProvider
{
    public Task<Guid?> GetProjectRoleIdAsync(Guid userId, Guid projectId, CancellationToken ct = default)
        => Task.FromResult<Guid?>(null);
}
