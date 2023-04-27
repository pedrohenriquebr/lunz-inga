using LuzInga.Domain.Services;
using LuzInga.Domain.ValueObjects;

namespace LuzInga.Infra.Services;

public class SubscriptionIdGenerator : ISubscriptionIdGenerator
{
    public SubscriptionId NextId() => Guid.NewGuid();
}