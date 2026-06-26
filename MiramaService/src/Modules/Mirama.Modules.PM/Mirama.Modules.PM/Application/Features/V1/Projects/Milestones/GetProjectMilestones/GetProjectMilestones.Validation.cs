using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.GetProjectMilestones;

internal class GetProjectMilestonesQueryValidator : AbstractValidator<GetProjectMilestonesQuery>
{
    public GetProjectMilestonesQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(50)
            .When(q => q.PageSize.HasValue);
    }
}
