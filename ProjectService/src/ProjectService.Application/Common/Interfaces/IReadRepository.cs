



namespace ProjectService.Application.Common.Interfaces;

public interface IReadRepository<T> where T : class // Use aggregate root from Domain layer as class later
{
    IQueryable<T> Query();
}