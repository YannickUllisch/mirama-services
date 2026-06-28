using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Priorities.GetProjectPriorities;

internal class GetProjectPrioritiesQueryValidator : AbstractValidator<GetProjectPrioritiesQuery>
{
    public GetProjectPrioritiesQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}
