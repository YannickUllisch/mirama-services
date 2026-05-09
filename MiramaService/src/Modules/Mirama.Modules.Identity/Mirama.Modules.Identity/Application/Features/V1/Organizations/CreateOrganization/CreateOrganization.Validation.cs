using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.CreateOrganization;

internal class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(c => c.Street).NotEmpty().MaximumLength(200);
        RuleFor(c => c.City).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Country).NotEmpty().MaximumLength(100);
        RuleFor(c => c.ZipCode).NotEmpty().MaximumLength(20);
        RuleFor(c => c.Logo).MaximumLength(500).When(c => c.Logo is not null);
    }
}
