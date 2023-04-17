namespace LuzInga.Domain.Common;

public abstract class BaseEntity<Tkey>
    where Tkey : IComparable
{
    public Tkey Id { get; protected set; }
}
