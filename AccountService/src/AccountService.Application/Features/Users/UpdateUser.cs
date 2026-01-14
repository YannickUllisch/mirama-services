

using System.Text.Json.Serialization;
using MediatR;
using ErrorOr;
using AccountService.Application.Infrastructure.Persistence;
using AccountService.Application.Domain.Aggregates.User;
using FluentValidation;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Application.Features.Users;

public class UpdateUserController : ApiControllerBase
{
    [HttpPut("user/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateUserCommand command)
    {
        var cmd = command with { Id = id };
        var result = await Mediator.Send(command);

        return result.Match(Ok, Problem);
    }
}

public sealed record UpdateUserCommand : IRequest<ErrorOr<UserResponse>>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; init; } = null;

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;

    [JsonPropertyName("contactEmail")]
    public string ContactEmail { get; init; } = string.Empty;

    [JsonPropertyName("contactPhoneNumber")]
    public string ContactPhoneNumber { get; init; } = string.Empty;

}

internal class UpdateUserRequestValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserRequestValidator(IGlobalRoleProvider roleProvider)
    {
        RuleFor(req => req.Id).NotEmpty();

        RuleFor(req => req.Email)
            .EmailAddress();

        RuleFor(req => req.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(25)
            .WithMessage("Name must be between 3 and 25 characters long");

        RuleFor(req => req.Role)
            .NotEmpty()
            .Must(role => roleProvider.AllowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid Role Provided");
    }
}

internal class UpdateUserCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateUserCommand, ErrorOr<UserResponse>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ErrorOr<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FindAsync([request.Id], cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User could not be found");
        }

        if (!Enum.TryParse<GlobalRole>(request.Role, true, out var parsedRole))
        {
            return Error.Validation("Invalid role value.");
        }

        user.Update(
            request.Name,
            request.Email,
            parsedRole,
            request.ContactEmail,
            request.ContactPhoneNumber,
            request.Image
        );

        await _context.SaveChangesAsync(cancellationToken);

        return user.MapResponse();
    }
}