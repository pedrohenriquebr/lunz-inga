namespace LuzInga.Domain.ValueObjects;

public sealed record SubscriptionId : IComparable<SubscriptionId> , IComparable
{
    public SubscriptionId(Guid guid)
    {
        Value  = guid;
    }
    public static implicit operator Guid (SubscriptionId value)
        => value.Value;

    public static implicit operator SubscriptionId (Guid value)
        => new(value);

    public Guid Value { get; }

    public int CompareTo(SubscriptionId? other)
    {
        return this.Value.CompareTo(other?.Value);
    }

    public int CompareTo(object? obj)
    {
        return this.CompareTo(((SubscriptionId?) obj));
    }
}
