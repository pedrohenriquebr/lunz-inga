using System.Net;
using LuzInga.Application.Common;
using LuzInga.Application.Services;
using LuzInga.Domain.Events;
using MediatR;

namespace LuzInga.Application.DomainEventsHandlers.NewsLetterSubscribed;


public sealed class SendWelcomeEmailAndConfirmationCode : IDomainEventHandler<NewsLetterSubscriptionCreatedEvent>
{
    private readonly IEmailProvider emailProvider;

    public SendWelcomeEmailAndConfirmationCode(IEmailProvider emailProvider)
    {
        this.emailProvider = emailProvider;
    }
    public Task Handle(NewsLetterSubscriptionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var name = notification.Subscription.Name;
        var confirmationLink = "http://localhost:3000"
                + Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION
                + "/confirm-email/" + WebUtility.UrlEncode(notification.Subscription.ConfirmationCode);
        string body = GetBody(name, confirmationLink);

        emailProvider.SendEmail(notification.Subscription.Email, "Welcome to LuzInga! ", body);
        return Unit.Task;
    }

    private static string GetBody(string name, string confirmationLink)
    {
        return $@"
<html>
<head>
    <title>Welcome to our platform!</title>
</head>
<body>
    <h1>Welcome, {name}!</h1>
    <p>Thank you for signing up for our platform. Before you can start using it, you need to confirm your email address by clicking on the following link:</p>
    <p><a href='{confirmationLink}'>Confirm email address</a></p>
    <p>If you did not sign up for our platform, please disregard this message.</p>
    <br>
    <p>Best regards,</p>
    <p>The LuzInga Team</p>
</body>
</html>
";
    }
}
