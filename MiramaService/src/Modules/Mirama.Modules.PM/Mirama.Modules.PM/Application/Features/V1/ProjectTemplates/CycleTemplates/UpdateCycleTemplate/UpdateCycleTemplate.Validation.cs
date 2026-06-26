using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.UpdateCycleTemplate;

internal class UpdateCycleTemplateCommandValidator : AbstractValidator<UpdateCycleTemplateCommand>
{
    public UpdateCycleTemplateCommandValidator()
    {
        RuleFor(x => x.ProjectTemplateId).NotEmpty();
        RuleFor(x => x.CycleTemplateId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DurationDays)
            .GreaterThan(0)
            .When(x => x.DurationDays.HasValue);
    }
}
