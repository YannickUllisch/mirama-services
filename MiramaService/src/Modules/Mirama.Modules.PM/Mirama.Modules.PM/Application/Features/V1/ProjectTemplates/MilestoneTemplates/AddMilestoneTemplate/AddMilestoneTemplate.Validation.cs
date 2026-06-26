using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.AddMilestoneTemplate;

internal class AddMilestoneTemplateCommandValidator : AbstractValidator<AddMilestoneTemplateCommand>
{
    public AddMilestoneTemplateCommandValidator()
    {
        RuleFor(x => x.ProjectTemplateId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DayOffset)
            .GreaterThanOrEqualTo(0);
    }
}
