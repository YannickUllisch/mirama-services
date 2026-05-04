using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Application.Common;

public sealed class GlobalRoleProvider : IGlobalRoleProvider
{
    IReadOnlyCollection<string> IGlobalRoleProvider.AllowedRoles => Enum.GetNames<GlobalRole>();
}
