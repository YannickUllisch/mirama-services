
namespace ProjectService.Api.Mapping;

public interface IMapper<TInput, TOutput>
{
    TOutput Map(TInput input);
}
