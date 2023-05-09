using LuzInga.Application.Common;
using LuzInga.Application.Usecases.NewsletterSubscription;
using LuzInga.Domain.Events;
using LuzInga.Domain.Services;
using MediatR;

namespace LuzInga.Application.DomainEventsHandlers.NewsLetterSubscribed;

public class RefreshConfirmationCode : IDomainEventHandler<NewsLetterSubscriptionEmailChangedEvent>
{
    private readonly IMediator mediator;

    public RefreshConfirmationCode(IMediator mediator)
    {
        this.mediator = mediator;
    }
    public async Task Handle(NewsLetterSubscriptionEmailChangedEvent @event, CancellationToken cancellationToken)
    {
       mediator.Enqueue(new RefreshConfirmationTokenCommand(@event.Subscription.Id));
       await Unit.Task;
    }
}
