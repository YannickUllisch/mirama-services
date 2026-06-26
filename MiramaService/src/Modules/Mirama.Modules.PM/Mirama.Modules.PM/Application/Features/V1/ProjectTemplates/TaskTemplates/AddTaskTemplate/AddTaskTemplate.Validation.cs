using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates.AddTaskTemplate;

internal class AddTaskTemplateCommandValidator : AbstractValidator<AddTaskTemplateCommand>
{
    public AddTaskTemplateCommandValidator()
    {
        RuleFor(x => x.ProjectTemplateId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Type).IsInEnum();

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0)
            .When(x => x.EstimatedHours.HasValue);
    }
}
