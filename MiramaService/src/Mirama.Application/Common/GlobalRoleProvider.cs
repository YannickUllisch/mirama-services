


using Mirama.Application.Common.Interfaces;
using Mirama.Application.Domain.Aggregates.User;

namespace Mirama.Application.Common;

public sealed class GlobalRoleProvider : IGlobalRoleProvider
{
    IReadOnlyCollection<string> IGlobalRoleProvider.AllowedRoles => Enum.GetNames<GlobalRole>();
}
