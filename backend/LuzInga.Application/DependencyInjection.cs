using LuzInga.Application.Services;
using LuzInga.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LuzInga.Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder
            .Services
                .AddBloomFilter();

        return builder;
    }


    

    public static IServiceCollection AddBloomFilter(this IServiceCollection collection)
    {
        using var scope = collection.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ILuzIngaContext>();
        var contactsList = context.NewsLetterSubscription.ToList().Select(contact => contact.Email).ToList();

        collection.AddSingleton<IBloomFilter>(
            provider =>
                DefaultBloomFilter.CreateFrom(
                    contactsList,
                    options =>
                    {
                        options.MinSize = 1_000;
                    }
                )
        );

        return collection;
    }

}
