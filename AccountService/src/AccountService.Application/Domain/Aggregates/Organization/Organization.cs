
using AccountService.Application.Domain.Abstractions.Core;
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

    public List<Member.Member> Members = [];

    public List<Invitation.Invitation> Invitations = [];


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
        Members.Add(member);

        return Result.Created;
    }

    public ErrorOr<Deleted> RemoveMember(Guid mid)
    {
        var memberId = new MemberId(mid);
        var member = Members.Find(m => m.Id == memberId);

        if (member == null)
        {
            return Error.NotFound("User not Found");
        }

        Members.Remove(member);

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
        return Members.Any(m => m.Id == memberId);
    }

    void ITenantOwned.SetTenantId(Guid tenantId)
    {
        if (TenantId != Guid.Empty)
        {
            throw new InvalidOperationException("OrganizationId already set.");
        }
        TenantId = tenantId;
    }

    private static string GenerateSlug(string input)
    {
        return input.Trim().ToLower().Replace(" ", "-");
    }
}
