using LuzInga.Domain.Entities;
using LuzInga.Domain.SharedKernel;

namespace LuzInga.Domain.Events;

public sealed class NewsLetterSubscriptionCreatedEvent : BaseEvent
{
    public NewsLetterSubscription Subscription { get;}
    
    public NewsLetterSubscriptionCreatedEvent(NewsLetterSubscription subscription) : base()
    {
        this.Subscription = subscription;
    }
}
