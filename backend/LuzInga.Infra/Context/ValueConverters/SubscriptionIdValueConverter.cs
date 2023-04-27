using LuzInga.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LuzInga.Infra.Context.ValueConverters;

public class SubscriptionIdValueConverter : ValueConverter<SubscriptionId, Guid>
{
    public SubscriptionIdValueConverter()
    :base(from => from.Value, to => new SubscriptionId(to))
    {
        
    }
}
