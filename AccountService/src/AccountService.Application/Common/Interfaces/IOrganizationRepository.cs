

using AccountService.Application.Domain.Organization;
using AccountService.Application.Domain.Organization.Invitation;
using AccountService.Application.Domain.Organization.Invitation.Valueobjects;
using AccountService.Application.Domain.Organization.ValueObjects;

namespace AccountService.Application.Common.Interfaces;

public interface IOrganizationRepository
{
    Organization GetOrganizationById(OrganizationId id);

    List<Member> GetOrganizationMembers(OrganizationId id);

    List<Invitation> GetInvitations(OrganizationId id);

    Invitation GetInvitationById(InvitationId id);
}