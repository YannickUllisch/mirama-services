using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.GetProjectMembers;

internal class GetProjectMembersQueryValidator : AbstractValidator<GetProjectMembersQuery>
{
    public GetProjectMembersQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(50)
            .When(q => q.PageSize.HasValue);
    }
}
