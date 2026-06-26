using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjects;

internal class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(50)
            .When(q => q.PageSize.HasValue);
    }
}
