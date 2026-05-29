

namespace Mirama.SharedKernel.Abstractions.Persistence;

public interface IGlobalRoleProvider
{
    IReadOnlyCollection<string> AllowedRoles { get; }
}