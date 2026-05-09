using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using InvitationEntity = Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation.Invitation;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using MemberEntity = Mirama.Modules.Identity.Domain.Aggregates.Organization.Member.Member;

namespace Mirama.Modules.Identity.Domain.Aggregates.Organization;

public class Organization : AggregateRoot<OrganizationId>, ITenantOwned
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Logo { get; private set; }
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public DateTime DateCreated { get; private set; }
    public Guid TenantId { get; private set; } = Guid.Empty;

    public List<MemberEntity> Members = [];
    public List<InvitationEntity> Invitations = [];

    private Organization(OrganizationDetails details)
    {
        Name = details.Name.Trim();
        Slug = GenerateSlug(details.Name);
        Logo = details.Logo;
        Street = details.Street.Trim();
        City = details.City.Trim();
        Country = details.Country.Trim();
        ZipCode = details.ZipCode.Trim();
        DateCreated = DateTime.UtcNow;
    }

    private Organization() { }

    public static Organization Create(OrganizationDetails details)
    {
        return new Organization(details) { Id = new OrganizationId(Guid.NewGuid()) };
    }

    public ErrorOr<Created> AddMember(MemberDetails details)
    {
        Members.Add(MemberEntity.Create(details));
        return Result.Created;
    }

    public ErrorOr<Deleted> RemoveMember(Guid mid)
    {
        var memberId = new MemberId(mid);
        var member = Members.Find(m => m.Id == memberId);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        Members.Remove(member);
        return Result.Deleted;
    }

    public bool HasMember(Guid mid)
    {
        var memberId = new MemberId(mid);
        return Members.Any(m => m.Id == memberId);
    }

    public void Update(OrganizationDetails details)
    {
        Name = details.Name.Trim();
        Slug = GenerateSlug(details.Name);
        Logo = details.Logo;
        Street = details.Street.Trim();
        City = details.City.Trim();
        Country = details.Country.Trim();
        ZipCode = details.ZipCode.Trim();
    }

    void ITenantOwned.SetTenantId(Guid tenantId)
    {
        if (TenantId != Guid.Empty)
            throw new InvalidOperationException("TenantId already set.");

        TenantId = tenantId;
    }

    private static string GenerateSlug(string input) =>
        input.Trim().ToLower().Replace(" ", "-");
}
