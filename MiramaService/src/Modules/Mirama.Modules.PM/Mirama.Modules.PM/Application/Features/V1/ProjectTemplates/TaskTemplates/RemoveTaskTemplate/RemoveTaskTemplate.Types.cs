using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates.RemoveTaskTemplate;

public sealed record RemoveTaskTemplateCommand(Guid ProjectTemplateId, Guid TaskTemplateId) : ICommand<ErrorOr<Deleted>>;
