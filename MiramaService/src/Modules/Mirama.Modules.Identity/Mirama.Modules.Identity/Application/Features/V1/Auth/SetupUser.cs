using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

[AllowAnonymous]
public class SetupUserController : ApiControllerBase
{
    [HttpPost("auth/setup")]
    public async Task<ActionResult<SetupUserResponse>> Post([FromBody] SetupUserCommand command)
    {
        var res = await this.Dispatcher.Send(command);
        return res.Match(Ok, Problem);
    }
}

public sealed record SetupUserCommand : ICommand<ErrorOr<SetupUserResponse>>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; init; }
}

internal class SetupUserCommandValidator : AbstractValidator<SetupUserCommand>
{
    public SetupUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
    }
}

internal class SetupUserCommandHandler(
    IdentityDbContext dbContext) : IRequestHandler<SetupUserCommand, ErrorOr<SetupUserResponse>>
{
    public async Task<ErrorOr<SetupUserResponse>> HandleAsync(SetupUserCommand request, CancellationToken ct)
    {
        var existingUser = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == new UserId(request.Id), ct);

        if (existingUser is not null)
            return Error.Conflict("User.AlreadyExists", "User already exists.");

        var freePlan = await dbContext.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == "Free", ct);

        if (freePlan is null)
            return Error.NotFound("Plan.NotFound", "Free plan not seeded.");

        var user = User.CreateWithId(
            new UserDetails(request.Name, request.Email, TenantRole.Owner, request.Image),
            request.Id);

        var now = DateTime.UtcNow;
        var tenant = Tenant.Create(
            request.Id,
            new TenantSettingsDetails(request.Name, ReceiveNotifications: true, BrandingColor: null, LogoUrl: null),
            new SubscriptionDetails(freePlan.Id, now, now.AddYears(100)));

        dbContext.Users.Add(user);
        dbContext.Tenants.Add(tenant);
        await dbContext.SaveChangesAsync(ct);

        return new SetupUserResponse
        {
            UserId = user.Id.Value,
            Name = user.Name,
            Email = user.Email,
            Image = user.Image,
            Role = user.Role.ToString(),
            TenantId = tenant.Id,
            Settings = new SetupTenantSettingsResponse
            {
                Name = tenant.Settings.Name,
                IsActive = tenant.Settings.IsActive,
                Timezone = tenant.Settings.Timezone,
                ReceiveNotifications = tenant.Settings.ReceiveNotifications,
            },
            Subscription = new SetupSubscriptionResponse
            {
                Status = tenant.Subscription.Status.ToString(),
                PlanId = freePlan.Id.Value,
                PlanName = freePlan.Name,
                Features = [.. freePlan.Features],
                PeriodStart = tenant.Subscription.PeriodStart,
                PeriodEnd = tenant.Subscription.PeriodEnd,
            },
        };
    }
}

public sealed record SetupUserResponse
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; init; }

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("settings")]
    public SetupTenantSettingsResponse Settings { get; init; } = default!;

    [JsonPropertyName("subscription")]
    public SetupSubscriptionResponse Subscription { get; init; } = default!;
}

public sealed record SetupTenantSettingsResponse
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; init; } = string.Empty;

    [JsonPropertyName("receiveNotifications")]
    public bool ReceiveNotifications { get; init; }
}

public sealed record SetupSubscriptionResponse
{
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("planId")]
    public Guid PlanId { get; init; }

    [JsonPropertyName("planName")]
    public string PlanName { get; init; } = string.Empty;

    [JsonPropertyName("features")]
    public List<string> Features { get; init; } = [];

    [JsonPropertyName("periodStart")]
    public DateTime PeriodStart { get; init; }

    [JsonPropertyName("periodEnd")]
    public DateTime PeriodEnd { get; init; }
}
