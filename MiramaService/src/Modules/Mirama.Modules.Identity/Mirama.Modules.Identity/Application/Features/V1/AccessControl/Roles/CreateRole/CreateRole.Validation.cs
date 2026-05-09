using FluentValidation;
using Mirama.Modules.Identity.Domain.Enums;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.CreateRole;

internal class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => c.Description is not null);

        RuleFor(c => c.Scope)
            .NotEmpty()
            .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
            .WithMessage("Scope must be 'Organization' or 'Project'");
    }
}
