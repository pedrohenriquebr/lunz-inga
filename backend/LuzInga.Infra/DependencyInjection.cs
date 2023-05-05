using System.Text;
using Bogus;
using LuzInga.Application;
using LuzInga.Application.Configuration;
using LuzInga.Application.Services;
using LuzInga.Domain;
using LuzInga.Domain.Entities;
using LuzInga.Domain.Factories;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel;
using LuzInga.Infra.Context;
using LuzInga.Infra.Services;
using LuzInga.Infra.Services.Redis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace LuzInga.Infra;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfra(this WebApplicationBuilder builder)
    {
        builder.Services
                        .AddConfiguration(builder.Configuration)
                        .AddRedis(builder.Configuration)
                        .AddServices()
                        .AddDbContext(builder.Configuration)
                        ;
        return builder;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection collection, IConfiguration config)
    {
        return collection
                    .Configure<RedisConfig>(opt =>
                        config.GetSection(nameof(RedisConfig))
                                .Bind(opt));
    }

    

    public static IServiceCollection AddServices(this IServiceCollection collection)
        => collection
                .AddScoped<ISubscriptionIdGenerator, SubscriptionIdGenerator>()
                .AddScoped<INewsLetterSubscriptionFactory, NewsLetterSubscriptionFactory>()
                .AddScoped<ISubscriptionConfirmationCodeFactory, DefaultSubscriptionConfirmationCodeFactory>();


    public static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        string redisConnectionString = config.GetConnectionString("Redis");
        var redisConfig = services
                                .BuildServiceProvider()
                                .GetRequiredService<IOptions<RedisConfig>>();

        var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);


        services.AddSingleton<IConnectionMultiplexer>(redisConnection);
        services.AddSingleton<IAuditLogger>(provider =>
                new AuditLogger(
                    provider.GetRequiredService<IConnectionMultiplexer>(),
                    BuildAuditPrefix(redisConfig)));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = BuildInstanceNamePrefix(redisConfig);
        });


        services.AddSession();
        services.AddResponseCaching();

        return services;
    }

    private static RedisKey BuildAuditPrefix(IOptions<RedisConfig> redisConfig)
    {
        return new StringBuilder(BuildInstanceNamePrefix(redisConfig))
                    .Append(redisConfig.Value.AuditListKey)
                    .ToString();
    }

    private static string BuildInstanceNamePrefix(IOptions<RedisConfig> redisConfig)
    {
        return new StringBuilder()
                    .Append(redisConfig.Value.ApplicationPrefixKey)
                    .Append(redisConfig.Value.KeyDelimiter)
                    .ToString();
    }

    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration config
    )
    {

        var connectionString = config.GetConnectionString("DefaultConnection");

        // Add DbContext to dependency injection container
        services.AddDbContext<LuzIngaContext>(
            options => options.UseSqlServer(connectionString)
        );


        // Generate fake Contacts and add to LuzIngaContext
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LuzIngaContext>();
            var factory = scope.ServiceProvider.GetRequiredService<INewsLetterSubscriptionFactory>();
            if (!context.NewsLetterSubscription.Any())
            {
                var fakeContacts = GenerateFakeContacts(50_000, factory);
                context.AddRange(fakeContacts);
                context.SaveChanges();
            }
        }

        services.AddScoped<ILuzIngaContext>(sp =>
                sp.GetRequiredService<LuzIngaContext>()
        );

        services.AddScoped<IUnitOfWork>(sp =>
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
                email: f.Person.Email,
                name: f.Person.FullName
            ));

        return faker.Generate(count);
    }
}
