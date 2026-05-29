using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

public sealed class TeamMember : Entity<TeamMemberId>
{
    public TeamId TeamId { get; private set; } = default!;
    public MemberId MemberId { get; private set; } = default!;
    public Guid OrganizationId { get; private set; }

    private TeamMember() { }

    internal static TeamMember Create(TeamId teamId, MemberId memberId, Guid organizationId)
    {
        return new TeamMember
        {
            Id = new TeamMemberId(Guid.NewGuid()),
            TeamId = teamId,
            MemberId = memberId,
            OrganizationId = organizationId
        };
    }
}
