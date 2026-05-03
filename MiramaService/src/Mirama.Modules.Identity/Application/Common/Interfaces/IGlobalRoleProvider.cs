

namespace Mirama.Modules.Identity.Application.Common.Interfaces;

public interface IGlobalRoleProvider
{
    IReadOnlyCollection<string> AllowedRoles { get; }
}