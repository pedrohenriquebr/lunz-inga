using LuzInga.Domain.Services;
using MediatR;

namespace LuzInga.Domain.SharedKernel;

public abstract class BaseEvent : INotification 
{
    public DateTime DateTimeCreated { get; }
    
    public BaseEvent()
    {
        DateTimeCreated = DateTimeProvider.Now;
    }
}