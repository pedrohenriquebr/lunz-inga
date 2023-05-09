using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LuzInga.Domain.SharedKernel;


public interface IEntity 
{
    public IReadOnlyCollection<BaseEvent> DomainEvents {get;}
    public void ClearDomainEvents();
}

public abstract class BaseEntity<Tkey> : IEntity
    where Tkey : IComparable
{
    public Tkey Id { get; protected set; }

    [JsonIgnore]
    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public interface IAggregateRoot
{
    
}