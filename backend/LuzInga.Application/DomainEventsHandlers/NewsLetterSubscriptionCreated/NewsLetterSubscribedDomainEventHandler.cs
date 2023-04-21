using LuzInga.Application.Common;
using LuzInga.Domain;
using LuzInga.Domain.Events;
using MediatR;

namespace LuzInga.Application.DomainEventsHandlers.NewsLetterSubscribed;


public class NewsLetterSubscriptionCreatedEventDomainEventHandler : DomainEventHandler<NewsLetterSubscriptionCreatedEvent>
{
    private readonly ILuzIngaContext context;

    public NewsLetterSubscriptionCreatedEventDomainEventHandler(ILuzIngaContext context)
    {
        this.context = context;
    }

    public override Task Handle(NewsLetterSubscriptionCreatedEvent notification, CancellationToken cancellationToken)
    {

        var subscription = this.context.NewsLetterSubscription.FirstOrDefault(d => d.Id == notification.Subscription.Id);       
        return Unit.Task;
    }
}
