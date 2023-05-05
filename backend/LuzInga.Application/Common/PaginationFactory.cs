using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Application.Common;
public sealed class Paginator
{
    private const int MIN_PAGE = 0;

    public static Task<PaginatedResponse<TEntity>> Paginate<TEntity>(DbSet<TEntity> entity, BasePaginated request)
        where TEntity : class
    {
        return Paginate<TEntity>(entity.AsQueryable(), request);
    }

    public static async Task<PaginatedResponse<TEntity>> Paginate<TEntity>(IQueryable<TEntity> entity, BasePaginated request)
        where TEntity : class
    {
        var queryable = entity
                        .Filter(request);
        var totalItems = await queryable.CountAsync();
        var result = await queryable
                        .Sort(request)
                        .Paginate(request)
                        .ToListAsync();

        return new PaginatedResponse<TEntity>()
        {
            Items = result,
            PageSize = result.Count(),
            Total = totalItems,
            NexPage = (int)(request.Offset < (totalItems / request.Limit) ? request.Offset + 1 : request.Offset),
            PreviousPage = (int)(request.Offset > MIN_PAGE ? request.Offset - 1 : MIN_PAGE),
            LastPage = request.Offset >= (totalItems / request.Limit),
            Page = Math.Clamp(request.Offset ?? 0, MIN_PAGE, (int)(totalItems / request.Limit))
        };
    }
}
