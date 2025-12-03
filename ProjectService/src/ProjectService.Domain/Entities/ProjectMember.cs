

using ProjectService.Domain.Enums;
using ProjectService.Domain.ValueObjects;

namespace ProjectService.Domain.Entities;

public class ProjectMember(ProjectRole role) : BaseEntity<UserId>
{
    public ProjectRole Role { get; private set; } = role;

    internal void SetRole(ProjectRole role) => Role = role;
}