


using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Aggregates.User;

namespace AccountService.Application.Common;

public sealed class GlobalRoleProvider : IGlobalRoleProvider
{
    IReadOnlyCollection<string> IGlobalRoleProvider.AllowedRoles => Enum.GetNames<GlobalRole>();
}
