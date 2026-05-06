using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

public sealed class Team : AggregateRoot<TeamId>
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public DateTime DateCreated { get; private set; }
    public Guid OrganizationId { get; private set; }

    public List<TeamMember> Members { get; private set; } = [];

    private Team() { }

    public static Team Create(string name, Guid organizationId)
    {
        return new Team
        {
            Id = new TeamId(Guid.NewGuid()),
            Name = name.Trim(),
            Slug = GenerateSlug(name),
            DateCreated = DateTime.UtcNow,
            OrganizationId = organizationId
        };
    }

    public void AddMember(MemberId memberId)
    {
        if (Members.Any(m => m.MemberId == memberId))
            return;

        Members.Add(TeamMember.Create(Id, memberId, OrganizationId));
    }

    public ErrorOr<Deleted> RemoveMember(MemberId memberId)
    {
        var member = Members.Find(m => m.MemberId == memberId);
        if (member is null)
            return Error.NotFound("Team.Member.NotFound", "Member not in team.");

        Members.Remove(member);
        return Result.Deleted;
    }

    public void Update(string name)
    {
        Name = name.Trim();
        Slug = GenerateSlug(name);
    }

    private static string GenerateSlug(string input) =>
        input.Trim().ToLower().Replace(" ", "-");
}
