using System.ComponentModel.DataAnnotations.Schema;
using LuzInga.Domain.Common;
using LuzInga.Domain.Events;

namespace LuzInga.Domain.Entities;

public sealed class Contact  : BaseAggregateRoot<int>
{
    public Contact() { }

    public Contact(int contactId, string email, string name) : this(email, name)
    {
        Id = contactId;
    }

    public Contact(string email, string name)
    {
        Email = email;
        Name = name;
    }


    public static Contact Create(string email, string name)
    {
        var newContact = new Contact(email, name);
        newContact.AddDomainEvent(new ContactCreatedEvent(){
            Email = newContact.Email,
            Name = newContact.Name
        });
        
        return newContact;
    }
    
    public string Email { get; private set; }
    public string Name { get; private set; }
}
