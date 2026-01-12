

namespace AccountService.Application.Common.Interfaces;

public interface IGlobalRoleProvider
{
    IReadOnlyCollection<string> AllowedRoles { get; }
}