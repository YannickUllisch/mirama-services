using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Persistence;

namespace Mirama.Modules.Identity.Application.Common.Models;

public sealed class GlobalRoleProvider : IGlobalRoleProvider
{
    IReadOnlyCollection<string> IGlobalRoleProvider.AllowedRoles => Enum.GetNames<TenantRole>();
}
