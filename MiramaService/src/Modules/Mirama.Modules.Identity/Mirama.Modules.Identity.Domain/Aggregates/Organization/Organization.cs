using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;
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
    public List<Invitation.Invitation> Invitations = [];

    private Organization() { }

    public static Organization Create(string name, string street, string city, string country, string zipCode)
    {
        return new Organization
        {
            Id = new OrganizationId(Guid.NewGuid()),
            Name = name.Trim(),
            Slug = GenerateSlug(name),
            Street = street.Trim(),
            City = city.Trim(),
            Country = country.Trim(),
            ZipCode = zipCode.Trim(),
            DateCreated = DateTime.UtcNow
        };
    }

    public ErrorOr<Created> AddMember(string name, string email, RoleId iamRoleId, UserId? userId = null)
    {
        var member = MemberEntity.Create(name, email, iamRoleId, userId);
        Members.Add(member);
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

    void ITenantOwned.SetTenantId(Guid tenantId)
    {
        if (TenantId != Guid.Empty)
            throw new InvalidOperationException("TenantId already set.");

        TenantId = tenantId;
    }

    private static string GenerateSlug(string input) =>
        input.Trim().ToLower().Replace(" ", "-");
}
