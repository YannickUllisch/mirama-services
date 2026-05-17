using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth.SetupUser;

internal class SetupUserCommandValidator : AbstractValidator<SetupUserCommand>
{
    public SetupUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
    }
}
