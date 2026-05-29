namespace Mirama.SharedKernel.Abstractions.Permissions;

public interface IProjectRoleProvider
{
    Task<Guid?> GetProjectRoleIdAsync(Guid userId, Guid projectId, CancellationToken ct = default);
}
