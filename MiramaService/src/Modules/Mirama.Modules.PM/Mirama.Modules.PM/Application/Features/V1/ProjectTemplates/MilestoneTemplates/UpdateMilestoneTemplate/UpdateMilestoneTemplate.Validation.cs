using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.UpdateMilestoneTemplate;

internal class UpdateMilestoneTemplateCommandValidator : AbstractValidator<UpdateMilestoneTemplateCommand>
{
    public UpdateMilestoneTemplateCommandValidator()
    {
        RuleFor(x => x.ProjectTemplateId).NotEmpty();
        RuleFor(x => x.MilestoneTemplateId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DayOffset)
            .GreaterThanOrEqualTo(0);
    }
}
