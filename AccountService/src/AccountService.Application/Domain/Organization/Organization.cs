

using AccountService.Application.Domain.Abstractions;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;
using ErrorOr;

namespace AccountService.Application.Domain.Organization;

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

    private Organization(string name, Address address)
    {
        Id = new OrganizationId(new Guid());
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required") : name.Trim();
        Slug = GenerateSlug(name);
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Created = DateTime.UtcNow;
    }

    private Organization() { }

    public static Organization Create(string name, Address address)
    {
        return new Organization(name, address);
    }

    public ErrorOr<Created> AddMember(UserId uid, OrganizationRole role)
    {
        Member member = Member.Create(this.Id, uid, role);
        _members.Add(member);

        return Result.Created;
    }

    public ErrorOr<Created> AddMembers(List<(UserId uid, OrganizationRole role)> members)
    {
        List<Member> membersToAdd = [];
        foreach (var (uid, role) in members)
        {
            Member member = Member.Create(this.Id, uid, role);
            membersToAdd.Add(member);
        }
        _members.AddRange(membersToAdd);

        return Result.Created;
    }

    public ErrorOr<Deleted> RemoveMember(MemberId mid)
    {
        Member? member = _members.Find(m => m.Id == mid);

        if (member == null)
        {
            return Error.NotFound("User not Found");
        }

        _members.Remove(member);

        return Result.Deleted;
    }

    public ErrorOr<Updated> UpdateAddress(Address newAddress)
    {
        if (!newAddress.IsValid())
        {
            return Error.Validation("Invalid Address Type provided");
        }

        Address = newAddress;

        return Result.Updated;
    }

    public bool HasMember(MemberId uid)
    {
        return _members.Any(m => m.Id == uid);
    }

    private static string GenerateSlug(string input)
    {
        return input.Trim().ToLower().Replace(" ", "-");
    }

}
