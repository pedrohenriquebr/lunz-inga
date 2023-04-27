using LuzInga.Domain.ValueObjects;

namespace LuzInga.Domain.Services;

public interface ISubscriptionTokenProvider
{
    public string GenerateConfirmationCode(SubscriptionId id);
}
