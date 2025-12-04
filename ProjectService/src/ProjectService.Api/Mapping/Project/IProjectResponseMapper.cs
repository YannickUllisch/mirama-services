
using ProjectService.Api.Contracts.Responses;
using ProjectService.Application.Projects.DTOs;

namespace ProjectService.Api.Mapping.Project;

public interface IProjectResponseMapper
{
    ProjectResponse MapToApi(ProjectDto dto);
}