using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Application.Common;
public sealed class PaginationFactory
{
    public static Task<PaginatedResponse<TEntity>> Create<TEntity>(DbSet<TEntity> entity, BasePaginated request)
        where TEntity : class
    {
        return Create<TEntity>(entity.AsQueryable(), request);
    }

    public static async Task<PaginatedResponse<TEntity>> Create<TEntity>(IQueryable<TEntity> entity, BasePaginated request)
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
            PreviousPage = (int)(request.Offset > 0 ? request.Offset - 1 : 0),
            LastPage = request.Offset >= (totalItems / request.Limit),
        };
    }
}
