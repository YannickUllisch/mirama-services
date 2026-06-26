using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.DeleteProjectTemplate;

public sealed record DeleteProjectTemplateCommand(Guid Id) : ICommand<ErrorOr<Deleted>>;
