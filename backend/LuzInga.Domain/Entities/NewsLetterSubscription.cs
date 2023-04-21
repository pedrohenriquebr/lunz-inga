using System.ComponentModel.DataAnnotations.Schema;
using LuzInga.Domain.SharedKernel;
using LuzInga.Domain.Events;

namespace LuzInga.Domain.Entities;

public sealed class NewsLetterSubscription  : BaseEntity<int>, IAggregateRoot
{

    public NewsLetterSubscription(int subscriptionId, string email, string name) : this(email, name)
    {
        Id = subscriptionId;
    }

    public NewsLetterSubscription(string email, string name)
    {
        Email = email;
        Name = name;
        
        AddDomainEvent(new NewsLetterSubscriptionCreatedEvent(this));
    }

    public static NewsLetterSubscription Create(string email, string name) => new NewsLetterSubscription(email, name);

    public string Email { get; private set; }
    public string Name { get; private set; }
}


public sealed class QueueEvent :BaseEntity<int>, IAggregateRoot
{
    public QueueEvent(int id)
    {
        Id = id;
    }

    public DateTime MyProperty { get; set; }
}