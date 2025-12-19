
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Domain.Aggregates.Organization.Member;
using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Domain.ValueObjects;
using ErrorOr;

namespace AccountService.Application.Domain.Aggregates.Organization;

public class Organization : AggregateRoot<OrganizationId>, ITenantOwned
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Logo { get; private set; }

    public Address Address { get; private set; } = default!;

    public Guid TenantId { get; private set; } = Guid.Empty; // Set in DB Context

    public IReadOnlyList<Member.Member> Members => _members.AsReadOnly();

    public IReadOnlyList<Invitation.Invitation> Invitations => _invitations.AsReadOnly();

    private readonly List<Member.Member> _members = [];

    private readonly List<Invitation.Invitation> _invitations = [];

    private Organization(string name, Address address)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name required") : name.Trim();
        Slug = GenerateSlug(name);
        Address = address;
    }

    private Organization() { }

    public static ErrorOr<Organization> Create(string name, string street, string city, string country, string zipCode)
    {
        var addressResult = Address.Create(street, city, country, zipCode);
        if (addressResult.IsError)
            return addressResult.Errors;

        var org = new Organization(name.Trim(), addressResult.Value);
        return org;
    }

    public ErrorOr<Created> AddMember(Guid uid, OrganizationRole role)
    {
        var userId = new UserId(uid);
        var member = Member.Member.Create(userId, role);
        _members.Add(member);

        return Result.Created;
    }

    public ErrorOr<Created> AddMembers(List<(Guid uid, OrganizationRole role)> members)
    {
        List<Member.Member> membersToAdd = [];
        foreach (var (uid, role) in members)
        {
            var userId = new UserId(uid);
            var member = Member.Member.Create(userId, role);
            membersToAdd.Add(member);
        }
        _members.AddRange(membersToAdd);

        return Result.Created;
    }

    public ErrorOr<Deleted> RemoveMember(Guid mid)
    {
        var memberId = new MemberId(mid);
        var member = _members.Find(m => m.Id == memberId);

        if (member == null)
        {
            return Error.NotFound("User not Found");
        }

        _members.Remove(member);

        return Result.Deleted;
    }

    public ErrorOr<Updated> UpdateAddress(string street, string city, string country, string zipCode)
    {
        var address = Address.Create(street, city, country, zipCode);

        return address.Match<ErrorOr<Updated>>(
            newAddress =>
            {
                Address = newAddress;
                return Result.Updated;
            },
            err => err
        );
    }

    public bool HasMember(Guid mid)
    {
        var memberId = new MemberId(mid);
        return _members.Any(m => m.Id == memberId);
    }

    private static string GenerateSlug(string input)
    {
        return input.Trim().ToLower().Replace(" ", "-");
    }

    void ITenantOwned.SetTenantId(Guid tenantId)
    {
        if (TenantId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        TenantId = tenantId;
    }
}
