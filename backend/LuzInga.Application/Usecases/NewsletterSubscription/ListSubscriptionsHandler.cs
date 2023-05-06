using LuzInga.Application.Common;
using LuzInga.Application.Common.CQRS;
using LuzInga.Application.Usecases.NewsletterSubscription.QueryObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace LuzInga.Application.Usecases.NewsletterSubscription.ListSubscriptions;


[Route(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
public sealed class ListSubscriptionsHandler : BaseApiQueryHandler<ListSubscriptionsQueryObject, PaginatedResponse<SubscriptionResponse>>
{
    private readonly IMediator mediator;
    public ListSubscriptionsHandler(IMediator mediator)
    {
        this.mediator = mediator;

    }

    [HttpGet]
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

        var subscriptionsResult = await mediator.Send(request);

        return subscriptionsResult;
    }

}
