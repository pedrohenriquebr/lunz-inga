using LuzInga.Domain.Entities;
using LuzInga.Domain.ValueObjects;

namespace LuzInga.Domain.Services;



public interface INewsLetterSubscriptionRepository : IRepository<NewsLetterSubscription, SubscriptionId>
{
    NewsLetterSubscription? GetByConfirmationId(string confirmationCode);
}

