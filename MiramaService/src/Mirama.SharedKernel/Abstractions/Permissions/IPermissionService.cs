using System.Security.Claims;

namespace Mirama.SharedKernel.Abstractions.Permissions;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(
        ClaimsPrincipal user,
        string[] required,
        Guid? projectId = null,
        CancellationToken ct = default);
}
