using LuzInga.Domain.ValueObjects;

namespace LuzInga.Domain.Services;

public interface ISubscriptionConfirmationCodeFactory
{
    public string Generate(SubscriptionId id);
}
