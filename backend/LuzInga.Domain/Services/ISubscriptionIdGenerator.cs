using LuzInga.Domain.ValueObjects;

namespace LuzInga.Domain.Services;

public interface ISubscriptionIdGenerator
{
    public  SubscriptionId NextId();
}
