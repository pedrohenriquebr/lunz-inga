using System.Net;
using LuzInga.Application.Common;
using LuzInga.Application.Services;
using LuzInga.Domain.Events;
using MediatR;

namespace LuzInga.Application.DomainEventsHandlers.NewsLetterSubscribed;

public class SendConfirmedSubscriptionEmail : IDomainEventHandler<NewsLetterSubscriptionConfirmedEmailEvent>
{
    private readonly IEmailProvider emailProvider;
    public SendConfirmedSubscriptionEmail(IEmailProvider emailProvider)
    {
        this.emailProvider = emailProvider;
    }
    public async Task Handle(NewsLetterSubscriptionConfirmedEmailEvent @event, CancellationToken cancellationToken)
    {
        var name = @event.Subscription.Name;
        string body = GetBody(name);

        emailProvider.SendEmail(@event.Subscription.Email, "Email confirmed, congratulations!", body);
        await Unit.Task;
    }

    private static string GetBody(string name)
    {
        return $@"
<html>
<head>
    <title>Welcome to our platform!</title>
</head>
<body>
    <h1>Welcome, {name}!</h1>
    <p>Congratulations, your email address has been successfully confirmed and your account is now fully activated!</p>
    <p>Now you can start using our platform to explore new possibilities and achieve your goals.</p>
    <p>If you have any questions or issues, please do not hesitate to contact us.</p>
    <br>
    <p>Best regards,</p>
    <p>The LuzInga Team</p>
</body>
</html>
";
    }
}
