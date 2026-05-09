using FluentValidation;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.CreatePolicy;

internal class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
{
    public CreatePolicyCommandValidator()
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

        RuleForEach(c => c.Statements).ChildRules(stmt =>
        {
            stmt.RuleFor(s => s.Action).NotEmpty().MaximumLength(100);
            stmt.RuleFor(s => s.Resource).NotEmpty().MaximumLength(200);
            stmt.RuleFor(s => s.Effect)
                .Must(e => Enum.TryParse<Effect>(e, ignoreCase: true, out _))
                .WithMessage("Effect must be 'Allow' or 'Deny'");
        });
    }
}
