using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.CreateProject;

internal class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
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

        RuleForEach(x => x.Members).ChildRules(member =>
        {
            member.RuleFor(m => m.MemberId).NotEmpty();
            member.RuleFor(m => m.RoleId).NotEmpty();
        });

        RuleForEach(x => x.Milestones).ChildRules(milestone =>
        {
            milestone.RuleFor(m => m.Title).NotEmpty().MaximumLength(200);
            milestone.RuleFor(m => m.DueDate).NotEmpty();
        });
    }
}
