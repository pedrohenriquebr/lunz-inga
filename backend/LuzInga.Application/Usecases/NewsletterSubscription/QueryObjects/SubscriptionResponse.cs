using SubscriptionEntity = LuzInga.Domain.Entities.NewsLetterSubscription;
namespace LuzInga.Application.Usecases.NewsletterSubscription.QueryObjects;

public sealed record SubscriptionResponse(
    string Email,
    string Name,
    DateTime DateTimeCreated,
    DateTime DateTimeUpdated,
    bool IsConfirmed,
    string? ConfirmationCode,
    DateTime? ConfirmationCodeExpiration,
    string? UnsubscriptionReason,
    DateTime? DateTimeConfirmed,
    DateTime? DateTimeUnsubscribed,
    DateTime? DateTimeReactivated
)
{
    public static implicit operator SubscriptionResponse(SubscriptionEntity entity)
    {
        return new SubscriptionResponse(
            entity.Email,
            entity.Name,
            entity.DateTimeCreated,
            entity.DateTimeUpdated,
            entity.IsConfirmed,
            entity.ConfirmationCode,
            entity.ConfirmationCodeExpiration,
            entity.UnsubscriptionReason,
            entity.DateTimeConfirmed,
            entity.DateTimeUnsubscribed,
            entity.DateTimeReactivated
        );
    }
};