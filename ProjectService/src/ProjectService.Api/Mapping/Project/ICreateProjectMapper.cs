
using ProjectService.Api.Contracts.Requests;

namespace ProjectService.Api.Mapping.Project;

public interface ICreateProjectMapper
{
    string MapToApi(CreateProjectRequest request);
}