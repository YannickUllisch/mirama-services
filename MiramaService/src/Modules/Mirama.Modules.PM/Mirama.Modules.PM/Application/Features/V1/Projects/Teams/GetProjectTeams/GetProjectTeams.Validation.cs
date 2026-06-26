using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.GetProjectTeams;

internal class GetProjectTeamsQueryValidator : AbstractValidator<GetProjectTeamsQuery>
{
    public GetProjectTeamsQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(50)
            .When(q => q.PageSize.HasValue);
    }
}
