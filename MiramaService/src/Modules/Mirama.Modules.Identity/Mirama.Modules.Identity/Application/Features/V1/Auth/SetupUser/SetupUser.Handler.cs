using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth.SetupUser;

[AllowAnonymous]
public class SetupUserController : ApiControllerBase
{
    [HttpPost("auth/setup")]
    public async Task<ActionResult<Created>> Post([FromBody] SetupUserCommand command)
    {
        var res = await this.Dispatcher.Send(command);
        return res.Match(ToCreated, Problem);
    }
}

internal class SetupUserCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<SetupUserCommand, ErrorOr<Created>>
{
    public async Task<ErrorOr<Created>> HandleAsync(SetupUserCommand request, CancellationToken ct)
    {
        var existingUser = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.LinkedExternalIds.Contains(request.Id), ct);

        if (existingUser is not null)
        {
            return Error.Conflict("User.AlreadyExists", "User already exists.");
        }

        var freePlan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == "Free", ct);

        if (freePlan is null)
        {
            return Error.NotFound("Plan.NotFound", "Free plan not seeded.");
        }

        var user = User.CreateWithExternalId(
            new UserDetails(request.Name, request.Email, TenantRole.Owner, request.Image),
            request.Id);

        var now = DateTime.UtcNow;
        var tenant = Tenant.Create(
            user.Id.Value,
            new TenantSettingsDetails(request.Name, ReceiveNotifications: true, BrandingColor: null, LogoUrl: null),
            new SubscriptionDetails(freePlan.Id, now, now.AddYears(100)));

        dbContext.Users.Add(user);
        dbContext.Tenants.Add(tenant);

        return Result.Created;
    }
}
