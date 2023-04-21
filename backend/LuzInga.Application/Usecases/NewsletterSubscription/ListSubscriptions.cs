using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using LuzInga.Application.Common;
using LuzInga.Application.Common.CQRS;
using LuzInga.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using SubscriptionEntity = LuzInga.Domain.Entities.NewsLetterSubscription;

namespace LuzInga.Application.Usecases.Contact.ListContacts;

public sealed class ListSubscriptionsHandler : QueryHandler<ListSubscriptionsRequest, PaginatedResponse<SubscriptionEntity>>
{
    private readonly ILuzIngaContext context;

    public ListSubscriptionsHandler(ILuzIngaContext context)
    {
        this.context = context;
    }

    [HttpGet(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
    [SwaggerOperation(
        Summary = "Paginate and filter all newslettersubscription",
        Description = "Return a page of newslettersubscription",
        OperationId = "NewsLetterSubscription.ListSubscriptions",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<PaginatedResponse<SubscriptionEntity>> HandleAsync(
        [FromQuery]
        ListSubscriptionsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await Paginator.Paginate(context.NewsLetterSubscription, request);
    }
}


public sealed class ListSubscriptionsRequest : BasePaginated
{
    public int? ContactId { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Email { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Name { get; set; }
}