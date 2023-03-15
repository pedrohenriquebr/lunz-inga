namespace LuzInga.Domain.Entities;

public class Contact
{
    public Contact() { }

    public Contact(int contactId, string email, string name)
    {
        ContactId = contactId;
        Email = email;
        Name = name;
    }

    public Contact(string email, string name)
    {
        Email = email;
        Name = name;
    }

    public int ContactId { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
}
