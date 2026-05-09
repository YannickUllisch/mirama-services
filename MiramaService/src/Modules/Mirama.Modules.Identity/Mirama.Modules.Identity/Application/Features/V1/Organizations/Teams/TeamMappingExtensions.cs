using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams;

internal static class TeamMapper
{
    internal static TeamResponse MapResponse(this Team team) => new()
    {
        Id = team.Id.Value,
        Name = team.Name,
        Slug = team.Slug,
        DateCreated = team.DateCreated,
        OrganizationId = team.OrganizationId,
        MemberIds = team.Members.Select(m => m.MemberId.Value).ToList()
    };
}
