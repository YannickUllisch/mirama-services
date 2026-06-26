using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.UpdateProject;

internal class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(200);

        RuleFor(x => x.StatusId)
            .NotEmpty();

        RuleFor(x => x.PriorityId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("End date must be on or after start date.");

        RuleFor(x => x.Budget)
            .GreaterThanOrEqualTo(0);

    }
}
