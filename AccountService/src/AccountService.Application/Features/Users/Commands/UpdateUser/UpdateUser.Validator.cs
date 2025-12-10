
using AccountService.Application.Common.Interfaces;
using FluentValidation;

namespace AccountService.Application.Features.Users.Commands.UpdateUser;

internal class UpdateUserRequestValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserRequestValidator(IGlobalRoleProvider roleProvider)
    {
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