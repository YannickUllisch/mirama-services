using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CreateProjectTemplate;

internal class CreateProjectTemplateCommandValidator : AbstractValidator<CreateProjectTemplateCommand>
{
    public CreateProjectTemplateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(200);

        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => x.Category is not null);
    }
}
