using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Common;
namespace LuzInga.Application.Usecases.NewsletterSubscription.QueryObjects;

public sealed class ListSubscriptionsQueryObject : BasePaginated, IQueryObject<PaginatedResponse<SubscriptionResponse>>
{

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Email { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Name { get; set; }
}
