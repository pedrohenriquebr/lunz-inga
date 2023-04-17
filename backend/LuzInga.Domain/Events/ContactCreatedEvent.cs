using LuzInga.Domain.Common;

namespace LuzInga.Domain.Events;

public sealed class ContactCreatedEvent : BaseEvent
{
    public string Email { get; init; }
    public string Name { get; init; }
}
