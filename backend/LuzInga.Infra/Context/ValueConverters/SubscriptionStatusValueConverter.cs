using LuzInga.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LuzInga.Infra.Context.ValueConverters;

public class SubscriptionStatusValueConverter : ValueConverter<SubscriptionStatus, string>
{
    public SubscriptionStatusValueConverter()
           :base(from => Enum.GetName(from) ?? string.Empty, to => Enum.Parse<SubscriptionStatus>(to))

    {
        
    }
}
