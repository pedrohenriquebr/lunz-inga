using LuzInga.Domain.Entities;
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
    DateTime? DateTimeReactivated,
    Guid SubscriptionId,
    SubscriptionStatus Status
)
{
    public static implicit operator SubscriptionResponse(SubscriptionEntity entity)
    {
        return new SubscriptionResponse(
            Email: entity.Email,
            Name: entity.Name,
            DateTimeCreated: entity.DateTimeCreated,
            DateTimeUpdated: entity.DateTimeUpdated,
            IsConfirmed: entity.IsConfirmed,
            ConfirmationCode: entity.ConfirmationCode,
            ConfirmationCodeExpiration: entity.ConfirmationCodeExpiration,
            UnsubscriptionReason: entity.UnsubscriptionReason,
            DateTimeConfirmed: entity.DateTimeConfirmed,
            DateTimeUnsubscribed: entity.DateTimeUnsubscribed,
            DateTimeReactivated: entity.DateTimeReactivated,
            SubscriptionId: entity.Id,
            Status: entity.Status
        );
    }
};