using Bogus;
using LuzInga.Application;
using LuzInga.Domain;
using LuzInga.Domain.Entities;
using LuzInga.Domain.Factories;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel;
using LuzInga.Infra.Context;
using LuzInga.Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuzInga.Infra;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfra(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices(builder.Configuration);

        return builder;
    }

    public static IServiceCollection AddMediator(this IServiceCollection collection)
    {
        var assembly = AppDomain.CurrentDomain.Load("LuzInga.Application");
        collection.AddMediatR(c => {
            c.RegisterServicesFromAssembly(assembly);
        });

        return collection;
    }

    public static IServiceCollection AddServices(this IServiceCollection collection)
        => collection
                .AddScoped<ISubscriptionIdGenerator,SubscriptionIdGenerator>()
                .AddScoped<INewsLetterSubscriptionFactory, NewsLetterSubscriptionFactory>()
                .AddScoped<ISubscriptionTokenProvider, SubscriptionTokenProvider>();

    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddStackExchangeRedisCache(options => {
            options.Configuration = config.GetConnectionString("Redis");
            options.InstanceName = "LuzInga_"; 
        });


        services.AddServices();
        services.AddMediator();
        // Add DbContext to dependency injection container
        services.AddDbContext<LuzIngaContext>(
            options => options.UseSqlServer(connectionString)
        );

        services.AddDbContext<LuzIngaContext>(options =>
            options
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Transient);


        // Generate fake Contacts and add to LuzIngaContext
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LuzIngaContext>();
            var factory  = scope.ServiceProvider.GetRequiredService<INewsLetterSubscriptionFactory>();
            if (!context.NewsLetterSubscription.Any())
            {
                var fakeContacts = GenerateFakeContacts(1_000,factory);
                context.AddRange(fakeContacts);
                context.SaveChanges();
            }
        }

        services.AddScoped<ILuzIngaContext>(sp => 
                sp.GetRequiredService<LuzIngaContext>()
                .WithMediator(sp.GetRequiredService<IMediator>())
        );

        return services;
    }

    private static IEnumerable<NewsLetterSubscription> GenerateFakeContacts(int count, INewsLetterSubscriptionFactory factory)
    {
        var faker = new Faker<NewsLetterSubscription>()
            .UseSeed(1314159)
            .CustomInstantiator(f => factory.CreateSubscription(
                email:f.Person.Email,
                name: f.Person.FullName
            ));

        return faker.Generate(count);
    }
}
