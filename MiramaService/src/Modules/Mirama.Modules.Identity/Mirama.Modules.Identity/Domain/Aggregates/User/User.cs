using ErrorOr;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.User;

public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Image { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public TenantRole Role { get; private set; }
    public bool IsOnboarded { get; private set; } = false;
    public List<Guid> LinkedExternalIds { get; private set; } = [];

    private User(UserDetails details)
    {
        this.Name = details.Name.Trim();
        this.Email = details.Email.Trim();
        this.Role = details.Role;
        this.Image = details.Image;
        this.IsOnboarded = false;
    }

    private User() { }

    public static User Create(UserDetails details)
    {
        return new User(details) { Id = new UserId(Guid.NewGuid()) };
    }

    public static User CreateWithExternalId(UserDetails details, Guid externalId)
    {
        var user = new User(details) { Id = new UserId(Guid.NewGuid()) };
        user.EmailVerified = DateTime.UtcNow;
        user.LinkedExternalIds.Add(externalId);
        return user;
    }

    public void Update(UserDetails details)
    {
        this.Name = details.Name.Trim();
        this.Email = details.Email.Trim();
        this.Role = details.Role;
        this.Image = details.Image;
    }

    public void VerifyEmail()
    {
        this.EmailVerified = DateTime.UtcNow;
    }

    public void HasBeenOnboarded()
    {
        this.IsOnboarded = true;
    }

    public ErrorOr<Success> LinkExternalId(Guid externalId)
    {
        if (this.EmailVerified is null)
            return Error.Forbidden("User.EmailNotVerified", "Cannot link provider to account with unverified email.");

        if (this.Id.Value == externalId || this.LinkedExternalIds.Contains(externalId))
            return Error.Conflict("User.ExternalId.AlreadyLinked", "External ID is already linked to this account.");

        this.LinkedExternalIds.Add(externalId);
        return Result.Success;
    }
}
