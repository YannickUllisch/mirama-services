using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.UpdatePolicy;

internal class UpdatePolicyCommandValidator : AbstractValidator<UpdatePolicyCommand>
{
    public UpdatePolicyCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => c.Description is not null);
    }
}
