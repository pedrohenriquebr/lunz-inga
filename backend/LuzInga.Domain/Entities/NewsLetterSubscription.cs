using LuzInga.Domain.SharedKernel;
using LuzInga.Domain.Events;
using LuzInga.Domain.ValueObjects;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel.Exceptions;

namespace LuzInga.Domain.Entities;



public sealed class NewsLetterSubscription : BaseEntity<SubscriptionId>, IAggregateRoot
{

    public void RefreshConfirmationCode(string confirmationCode)
    {
        this.ConfirmationCode = confirmationCode;
        this.ConfirmationCodeExpiration = _generateConfirmationCodExpiration();
        AddDomainEvent(new ConfirmationCodeRefreshedEvent(this.Id, this.ConfirmationCode, this.ConfirmationCodeExpiration));
    }
    
    public void ChangeEmailAddress(string email)
    {
        Email = email;
        DateTimeUpdated = DateTimeProvider.Now;
        AddDomainEvent(new NewsLetterSubscriptionEmailChangedEvent(this));
    }

    public void ChangeName(string name)
    {
        Name = name;
        DateTimeUpdated = DateTimeProvider.Now;
        AddDomainEvent(new NewsLetterSubscriptionUpdatedEvent(this));
    }

    public void Unsubscribe(string? reason)
    {
        DateTimeUpdated = DateTimeProvider.Now;
        DateTimeUnsubscribed = DateTimeProvider.Now;
        UnsubscriptionReason = reason;
        IsConfirmed = false;
        Status = SubscriptionStatus.Unsubscribed;
        AddDomainEvent(new NewsLetterSubscriptionUnsubscribedEvent(this));
    }

    public void Reactivate()
    {
        DateTimeUpdated = DateTimeProvider.Now;
        DateTimeReactivated = DateTimeProvider.Now;
        IsConfirmed = true;
        Status = SubscriptionStatus.Confirmed;
        AddDomainEvent(new NewsLetterSubscriptionReactivatedEvent(this));
    }

    public void ConfirmEmail()
    {
        var today = DateTimeProvider.Now;

        if (IsConfirmed)
        {
            throw new GlobalApplicationException(ApplicationExceptionType.Business, "Subscription has already been confirmed");
        }

        if (ConfirmationCodeExpiration != null && today > ConfirmationCodeExpiration)
        {
            AddDomainEvent(new NewsLetterSubscriptionConfirmationCodeExpiredEvent(this));
            throw new ConfirmationCodeExpiredException();
        }

        IsConfirmed = true;
        DateTimeUpdated = today;
        DateTimeConfirmed = today;
        ConfirmationCode = null;
        ConfirmationCodeExpiration = null;
        Status = SubscriptionStatus.Confirmed;
        AddDomainEvent(new NewsLetterSubscriptionConfirmedEmailEvent(this));
    }


    private DateTime _generateConfirmationCodExpiration()
    {
        return DateTimeProvider.Now + TimeSpan.FromHours(Constants.CONFIRMATION_TOKEN_EXPIRATION_HOURS);
    }

    protected NewsLetterSubscription()
    {
        
    }
    public NewsLetterSubscription(SubscriptionId id,string email,string name,string confirmationCode)
    {

        Id = id;
        Email = email;
        Name = name;
        DateTimeCreated = DateTimeProvider.Now;
        DateTimeUpdated = DateTimeProvider.Now;
        IsConfirmed = false;
        ConfirmationCode = confirmationCode;
        ConfirmationCodeExpiration = _generateConfirmationCodExpiration();
        UnsubscriptionReason = null;
        DateTimeConfirmed = null;
        DateTimeUnsubscribed = null;
        DateTimeReactivated = null;
        Status = SubscriptionStatus.Subscribed;
        AddDomainEvent(new NewsLetterSubscriptionCreatedEvent(this));
    }


    public string Email { get; private set; }
    public string Name { get; private set; }
    public DateTime DateTimeCreated { get; private set; }
    public DateTime DateTimeUpdated { get; private set; }
    public bool IsConfirmed { get; private set; }
    public string? ConfirmationCode { get; private set; }
    public DateTime? ConfirmationCodeExpiration { get; private set; }
    public string? UnsubscriptionReason { get; private set; }
    public DateTime? DateTimeConfirmed { get; private set; }
    public DateTime? DateTimeUnsubscribed { get; private set; }
    public DateTime? DateTimeReactivated { get; private set; }
    public SubscriptionStatus Status { get; private set; }
}


public enum SubscriptionStatus
{
    Subscribed,
    Confirmed,
    Unsubscribed
}