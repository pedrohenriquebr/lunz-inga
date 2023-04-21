using LuzInga.Domain.SharedKernel;
using MediatR;

namespace LuzInga.Application.Common;

public abstract class DomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : BaseEvent
{
    public abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
}
