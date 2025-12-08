
using AccountService.Domain.Organization;
using AccountService.Domain.Organization.Invitation;
using AccountService.Domain.Organization.Invitation.ValueObjects;
using AccountService.Domain.Organization.ValueObjects;

namespace AccountService.Application.Interfaces;


public interface IOrganizationRepository
{
    Organization GetOrganizationById(OrganizationId id);

    List<Member> GetOrganizationMembers(OrganizationId id);

    List<Invitation> GetInvitations(OrganizationId id);

    Invitation GetInvitationById(InvitationId id);
}