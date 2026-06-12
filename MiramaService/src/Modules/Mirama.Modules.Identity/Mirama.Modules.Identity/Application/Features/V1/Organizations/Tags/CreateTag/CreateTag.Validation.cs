using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Tags.CreateTag;

internal class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Color).MaximumLength(7).Matches(@"^#[0-9A-Fa-f]{6}$").When(c => c.Color is not null);
        RuleFor(c => c.Description).MaximumLength(500);
        RuleFor(c => c.Scope).GreaterThanOrEqualTo(0);
    }
}
