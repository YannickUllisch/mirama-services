

using AccountService.Application.Common.Models;

namespace AccountService.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize)
    {
        return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}