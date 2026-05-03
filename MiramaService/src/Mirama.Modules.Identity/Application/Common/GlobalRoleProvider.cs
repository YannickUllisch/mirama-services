


using Mirama.Domain.Aggregates.User;
using Mirama.Modules.Identity.Application.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Common;

public sealed class GlobalRoleProvider : IGlobalRoleProvider
{
    IReadOnlyCollection<string> IGlobalRoleProvider.AllowedRoles => Enum.GetNames<GlobalRole>();
}
