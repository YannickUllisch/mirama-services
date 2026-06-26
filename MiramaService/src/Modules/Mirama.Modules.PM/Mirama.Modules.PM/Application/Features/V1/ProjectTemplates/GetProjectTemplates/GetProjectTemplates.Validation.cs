using FluentValidation;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.GetProjectTemplates;

internal class GetProjectTemplatesQueryValidator : AbstractValidator<GetProjectTemplatesQuery>
{
    public GetProjectTemplatesQueryValidator()
    {
        RuleFor(q => q.PageSize)
            .LessThanOrEqualTo(50)
            .When(q => q.PageSize.HasValue);
    }
}
