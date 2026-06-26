using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.RemoveMilestoneTemplate;

public sealed record RemoveMilestoneTemplateCommand(Guid ProjectTemplateId, Guid MilestoneTemplateId) : ICommand<ErrorOr<Deleted>>;
