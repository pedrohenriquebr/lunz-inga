using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace LuzInga.Application.Common;

public class BasePaginated : IQuerySort, IQueryPaging
{
    public string Sort { get; set; } = "name";
    [QueryOperator(Max = 50)]
    public int? Limit { get; set; } = 10;

    [QueryOperator(HasName = "Page")]
    public int? Offset { get; set; } = 0;
}
