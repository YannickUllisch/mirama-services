using FluentValidation;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;

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
        RuleFor(c => c.Region)
            .Must(r => Enum.IsDefined(typeof(OrganizationRegion), r))
            .WithMessage("Region must be a valid organization region.");
        RuleFor(c => c.Logo).MaximumLength(500).When(c => c.Logo is not null);

        RuleFor(c => c.PrimaryColor)
            .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")
            .WithMessage("PrimaryColor must be a valid hex color (e.g. #FFF or #FFFFFF).")
            .When(c => c.PrimaryColor is not null);

        RuleFor(c => c.AccentColor)
            .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")
            .WithMessage("AccentColor must be a valid hex color (e.g. #FFF or #FFFFFF).")
            .When(c => c.AccentColor is not null);
    }
}
