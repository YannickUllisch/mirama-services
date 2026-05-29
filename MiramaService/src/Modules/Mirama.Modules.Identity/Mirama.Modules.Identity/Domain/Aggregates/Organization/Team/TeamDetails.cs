namespace Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;

public sealed record TeamDetails(
    string Name,
    Guid OrganizationId
);
