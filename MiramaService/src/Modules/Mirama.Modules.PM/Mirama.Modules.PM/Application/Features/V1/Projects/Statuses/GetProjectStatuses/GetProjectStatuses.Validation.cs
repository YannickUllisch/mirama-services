using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses.GetProjectStatuses;

internal class GetProjectStatusesQueryValidator : AbstractValidator<GetProjectStatusesQuery>
{
    public GetProjectStatusesQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}
