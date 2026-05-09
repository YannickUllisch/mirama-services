using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Tenants.UpdateTenantSettings;

internal class UpdateTenantSettingsCommandValidator : AbstractValidator<UpdateTenantSettingsCommand>
{
    public UpdateTenantSettingsCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Timezone)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.BrandingColor)
            .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")
            .WithMessage("BrandingColor must be a valid hex color (e.g. #FFF or #FFFFFF).")
            .When(c => c.BrandingColor is not null);

        RuleFor(c => c.LogoUrl)
            .MaximumLength(500)
            .When(c => c.LogoUrl is not null);
    }
}
