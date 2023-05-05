using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Behaviors;
using LuzInga.Application.Services;
using LuzInga.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LuzInga.Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder
            .Services
                .AddMediator()
                .AddBloomFilter();

        return builder;
    }


    public static IServiceCollection AddMediator(this IServiceCollection collection)
    {
        var assembly = AppDomain.CurrentDomain.Load("LuzInga.Application");
        collection
        .AddMediatR(c =>c.RegisterServicesFromAssembly(assembly))
        .AddValidatorsFromAssembly(assembly)
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        return collection;
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
