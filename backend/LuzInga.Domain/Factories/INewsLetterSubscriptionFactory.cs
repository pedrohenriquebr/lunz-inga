using LuzInga.Domain.Entities;
using LuzInga.Domain.Services;

namespace LuzInga.Domain.Factories;

public interface INewsLetterSubscriptionFactory
{
    public NewsLetterSubscription CreateSubscription(string name, string email);
}


