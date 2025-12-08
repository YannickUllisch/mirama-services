
using AccountService.Domain.Common;
using AccountService.Domain.Organization.ValueObjects;
using AccountService.Domain.User.ValueObjects;

namespace AccountService.Domain.Organization;

public class Organization : AuditableEntity, IHasDomainEvent
{
    public OrganizationId Id { get; private set; } = default!;

    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Logo { get; private set; }

    public Address Address { get; private set; } = default!;

    public IReadOnlyList<Member> Members => _members.AsReadOnly();

    public IReadOnlyList<Invitation.Invitation> Invitations => _invitations.AsReadOnly();

    public List<DomainEvent> DomainEvents => [];

    private readonly List<Member> _members = [];

    private readonly List<Invitation.Invitation> _invitations = [];

    private Organization(string name, Address address, string createdBy)
    {
        Id = new OrganizationId(new Guid());
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required") : name.Trim();
        Slug = GenerateSlug(name);
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Created = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    private Organization() { }

    public static Organization Create(string name, Address address, string createdBy)
    {
        return new Organization(name, address, createdBy);
    }

    public void AddMember(UserId uid, OrganizationRole role, string modifiedBy)
    {
        Member member = Member.Create(this.Id, uid, role, modifiedBy);
        _members.Add(member);
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    public void AddMembers(List<(UserId uid, OrganizationRole role)> members, string modifiedBy)
    {
        List<Member> membersToAdd = [];
        foreach (var (uid, role) in members)
        {
            Member member = Member.Create(this.Id, uid, role, modifiedBy);
            membersToAdd.Add(member);
        }
        _members.AddRange(membersToAdd);
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    public void RemoveMember(MemberId mid, string removedBy)
    {
        Member? member = _members.Find(m => m.Id == mid)
            ?? throw new ArgumentException($"Member with ID ${mid} could not be found");

        _members.Remove(member);
        LastModified = DateTime.UtcNow;
        LastModifiedBy = removedBy;
    }

    public void UpdateAddress(Address newAddress, string modifiedBy)
    {
        if (!newAddress.IsValid())
        {
            throw new ArgumentException("Invalid address");
        }

        Address = newAddress;
        LastModified = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    private static string GenerateSlug(string input)
    {
        // Simple slug generator
        return input.Trim().ToLower().Replace(" ", "-");
    }

}
