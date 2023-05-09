using LuzInga.Application.Common;
using LuzInga.Domain.Events;
using MediatR;

namespace LuzInga.Application.DomainEventsHandlers.NewsLetterSubscribed;

public sealed class SendConfirmationCodeEmail : IDomainEventHandler<ConfirmationCodeRefreshedEvent>
{
    public async Task Handle(ConfirmationCodeRefreshedEvent @event, CancellationToken cancellationToken)
    {
        
        await Unit.Task;
    }
}