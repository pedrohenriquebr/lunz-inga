using LuzInga.Domain.Entities;
using LuzInga.Domain.Factories;
using LuzInga.Domain.Services;

namespace LuzInga.Infra.Services;

public class NewsLetterSubscriptionFactory : INewsLetterSubscriptionFactory
{
    private readonly ISubscriptionConfirmationCodeFactory tokenProvider;
    private readonly ISubscriptionIdGenerator idGenerator;

    public NewsLetterSubscriptionFactory(ISubscriptionConfirmationCodeFactory tokenProvider, ISubscriptionIdGenerator idGenerator)
    {
        this.tokenProvider = tokenProvider;
        this.idGenerator = idGenerator;
    }

    public NewsLetterSubscription CreateSubscription(string name, string email)
    {
        var id = idGenerator.NextId();
        var confirmCode = tokenProvider.Generate(id);
        var expirationDate = DateTimeProvider.Now + TimeSpan.FromHours(Constants.CONFIRMATION_TOKEN_EXPIRATION_HOURS);

        return new NewsLetterSubscription(
            id,
            email,
            name,
            confirmCode,
            expirationDate
        );
    }
}
