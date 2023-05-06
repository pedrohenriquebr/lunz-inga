using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Common;
using LuzInga.Application.Usecases.NewsletterSubscription.ListSubscriptions;
using LuzInga.Domain;

namespace LuzInga.Application.Usecases.NewsletterSubscription.QueryObjects;

public class ListNewsletterSubscriptionQueryObjectHandler : IQueryObjectHandler<ListSubscriptionsQueryObject, PaginatedResponse<SubscriptionResponse>>
{
    private readonly ILuzIngaContext luzIngaContext;

    public ListNewsletterSubscriptionQueryObjectHandler(ILuzIngaContext luzIngaContext)
    {
        this.luzIngaContext = luzIngaContext;
    }

    public async Task<PaginatedResponse<SubscriptionResponse>> Handle(ListSubscriptionsQueryObject request, CancellationToken cancellationToken)
    {
        var rs = await Paginator.Paginate(luzIngaContext.NewsLetterSubscription, request);

        return new PaginatedResponse<SubscriptionResponse>()
        {
            NexPage = rs.NexPage,
            LastPage = rs.LastPage,
            PreviousPage = rs.PreviousPage,
            PageSize = rs.PageSize,
            Total = rs.Total,
            Items = rs.Items.Select(d => (SubscriptionResponse)d).ToList(),
            Page = rs.Page
        };
    }
}