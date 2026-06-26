using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.RemoveCycleTemplate;

public sealed record RemoveCycleTemplateCommand(Guid ProjectTemplateId, Guid CycleTemplateId) : ICommand<ErrorOr<Deleted>>;
