using LuzInga.Application.Common;
using LuzInga.Application.Common.CQRS;
using LuzInga.Application.Usecases.NewsletterSubscription.QueryObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace LuzInga.Application.Usecases.NewsletterSubscription.ListSubscriptions;

public sealed class ListSubscriptionsHandler : QueryHandler<ListSubscriptionsQueryObject, PaginatedResponse<SubscriptionResponse>>
{
    private readonly IMediator mediator;
    public ListSubscriptionsHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
    [ResponseCache(Duration = 30)]
    [SwaggerOperation(
        Summary = "Paginate and filter all newslettersubscription",
        Description = "Return a page of newslettersubscription",
        OperationId = "NewsLetterSubscription.ListSubscriptions",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<PaginatedResponse<SubscriptionResponse>> HandleAsync(
        [FromQuery]
        ListSubscriptionsQueryObject request,
        CancellationToken cancellationToken = default)
    {
        
        return await mediator.Send(request);
    }

}
