using System.Text;
using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using LuzInga.Application.Common;
using LuzInga.Application.Common.CQRS;
using LuzInga.Application.Extensions;
using LuzInga.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;


using SubscriptionEntity = LuzInga.Domain.Entities.NewsLetterSubscription;
namespace LuzInga.Application.Usecases.NewsletterSubscription.ListContacts;

public sealed class ListSubscriptionsHandler : QueryHandler<ListSubscriptionsRequest, PaginatedResponse<SubscriptionResponse>>
{
    private readonly ILuzIngaContext context;
    private readonly IDistributedCache cache;

    public ListSubscriptionsHandler(ILuzIngaContext context, IDistributedCache cache)
    {
        this.context = context;
        this.cache = cache;
    }

    [HttpGet(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    [SwaggerOperation(
        Summary = "Paginate and filter all newslettersubscription",
        Description = "Return a page of newslettersubscription",
        OperationId = "NewsLetterSubscription.ListSubscriptions",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<PaginatedResponse<SubscriptionResponse>> HandleAsync(
        [FromQuery]
        ListSubscriptionsRequest request,
        CancellationToken cancellationToken = default)
    {
        
        var rs = await Paginator.Paginate(context.NewsLetterSubscription, request);

        return new PaginatedResponse<SubscriptionResponse>()
        {
            NexPage = rs.NexPage,
            LastPage = rs.LastPage,
            PreviousPage = rs.PreviousPage,
            PageSize = rs.PageSize,
            Total = rs.Total,
            Items = rs.Items.Select(d => (SubscriptionResponse)d).ToList()
        };
    }

}


public sealed class ListSubscriptionsRequest : BasePaginated
{

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Email { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Name { get; set; }
}


public sealed record SubscriptionResponse(
    string Email,
    string Name,
    DateTime DateTimeCreated,
    DateTime DateTimeUpdated,
    bool IsConfirmed,
    string? ConfirmationCode,
    DateTime? ConfirmationCodeExpiration,
    string? UnsubscriptionReason,
    DateTime? DateTimeConfirmed,
    DateTime? DateTimeUnsubscribed,
    DateTime? DateTimeReactivated
)
{
    public static implicit operator SubscriptionResponse(SubscriptionEntity entity)
    {
        return new SubscriptionResponse(
            entity.Email,
            entity.Name,
            entity.DateTimeCreated,
            entity.DateTimeUpdated,
            entity.IsConfirmed,
            entity.ConfirmationCode,
            entity.ConfirmationCodeExpiration,
            entity.UnsubscriptionReason,
            entity.DateTimeConfirmed,
            entity.DateTimeUnsubscribed,
            entity.DateTimeReactivated
        );
    }
};