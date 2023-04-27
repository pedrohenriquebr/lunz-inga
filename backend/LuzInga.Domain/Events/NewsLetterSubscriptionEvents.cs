using LuzInga.Domain.Entities;
using LuzInga.Domain.SharedKernel;

namespace LuzInga.Domain.Events;

public sealed class NewsLetterSubscriptionCreatedEvent : BaseEvent
{
    public NewsLetterSubscription Subscription { get;}
    
    public NewsLetterSubscriptionCreatedEvent(NewsLetterSubscription subscription)
    {
        this.Subscription = subscription;
    }
}

public sealed class NewsLetterSubscriptionUpdatedEvent : BaseEvent
{
    public NewsLetterSubscription Subscription { get; }

    public NewsLetterSubscriptionUpdatedEvent(NewsLetterSubscription subscription)
    {
        this.Subscription = subscription;
    }
}


public sealed class NewsLetterSubscriptionUnsubscribedEvent :BaseEvent
{
    public NewsLetterSubscription Subscription { get; }

    public NewsLetterSubscriptionUnsubscribedEvent(NewsLetterSubscription subscription)
    {
        Subscription = subscription;
    }
}
public sealed class NewsLetterSubscriptionReactivatedEvent :BaseEvent
{
    public NewsLetterSubscription Subscription { get; }

    public NewsLetterSubscriptionReactivatedEvent(NewsLetterSubscription subscription)
    {
        Subscription = subscription;
    }
}
public sealed class NewsLetterSubscriptionConfirmedEmailEvent :BaseEvent
{
    public NewsLetterSubscription Subscription { get; }

    public NewsLetterSubscriptionConfirmedEmailEvent(NewsLetterSubscription subscription)
    {
        Subscription = subscription;
    }
}

public sealed class NewsLetterSubscriptionConfirmationCodeExpiredEvent :BaseEvent
{
    public NewsLetterSubscription Subscription { get; }

    public NewsLetterSubscriptionConfirmationCodeExpiredEvent(NewsLetterSubscription subscription)
    {
        Subscription = subscription;
    }
}