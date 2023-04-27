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

public sealed class ListSubscriptionsHandler : QueryHandler<ListSubscriptionsRequest, PaginatedResponse<SubscriptionEntity>>
{
    private readonly ILuzIngaContext context;
    private readonly IDistributedCache cache;

    public ListSubscriptionsHandler(ILuzIngaContext context, IDistributedCache cache)
    {
        this.context = context;
        this.cache = cache;
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
        var key = CreateKey(request); 
        var cacheResult = await this.cache.GetRecordAsync<PaginatedResponse<SubscriptionEntity>>(key);
        
        if(cacheResult is not null)
            return cacheResult;

        var result = await Paginator.Paginate(context.NewsLetterSubscription, request);

        await this.cache.SetRecordAsync(key, result);

        return result;
    }

    [NonAction]
    public string CreateKey(ListSubscriptionsRequest request)
    {
        var sb = new StringBuilder();
        sb.Append("ListSubscriptions_");
        
        if (!string.IsNullOrEmpty(request.Email))
        {
            sb.Append($"Email_{request.Email}_");
        }
        
        if (!string.IsNullOrEmpty(request.Name))
        {
            sb.Append($"Name_{request.Name}_");
        }
        
        sb.Append($"Page_{request.Offset}_");
        sb.Append($"PageSize_{request.Limit}");
        
        return sb.ToString();
    }

}


public sealed class ListSubscriptionsRequest : BasePaginated
{

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Email { get; set; }

    [QueryOperator(Operator = WhereOperator.StartsWith, CaseSensitive = false)]
    public string? Name { get; set; }
}