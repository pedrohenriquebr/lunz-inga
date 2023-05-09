using System.Text;
using Bogus;
using Hangfire;
using LuzInga.Application;
using LuzInga.Application.Configuration;
using LuzInga.Application.Services;
using LuzInga.Domain;
using LuzInga.Domain.Entities;
using LuzInga.Domain.Factories;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel;
using LuzInga.Domain.ValueObjects;
using LuzInga.Infra.Context;
using LuzInga.Infra.Services;
using LuzInga.Infra.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LuzInga.Infra;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfra(this WebApplicationBuilder builder)
    {
        builder.Services
                        .AddConfiguration(builder.Configuration)
                        .AddServices()
                        .AddRedis(builder.Configuration)
                        .AddDbContext(builder.Configuration)
                        .AddRepositories()
                        .AddHangfire(c => {
                            var serializerOptions = new JsonSerializerSettings(){
                                TypeNameHandling = TypeNameHandling.All,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            };

                            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                            c.UseSqlServerStorage(connectionString);
                            c.UseSerializerSettings(serializerOptions);
                        })
                        .AddHangfireServer();

        return builder;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection collection, IConfiguration config)
    {
        return collection
                    .Configure<RedisConfig>(opt =>
                        config.GetSection(nameof(RedisConfig))
                                .Bind(opt))
                    .Configure<EmailProviderConfig>(opt => 
                            config.GetSection(nameof(EmailProviderConfig))
                                .Bind(opt));
    }



    public static IServiceCollection AddServices(this IServiceCollection collection)
        => collection
                .AddSingleton<IRedisKeyFactory, RedisKeyFactory>()
                .AddScoped<ISubscriptionIdGenerator, SubscriptionIdGenerator>()
                .AddScoped<INewsLetterSubscriptionFactory, NewsLetterSubscriptionFactory>()
                .AddScoped<ISubscriptionConfirmationCodeFactory, DefaultSubscriptionConfirmationCodeFactory>()
                .AddTransient<IEmailProvider, EmailProvider>();

    public static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration config
    )
    {


        string redisConnectionString = config.GetConnectionString("Redis");
        var serviceProvider = services.BuildServiceProvider();

        var redisConfig = serviceProvider
                                .GetRequiredService<IOptions<RedisConfig>>();
        
        var keyFactory = serviceProvider.GetRequiredService<IRedisKeyFactory>();
        
        var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);


        services.AddSingleton<IConnectionMultiplexer>(redisConnection);
        services.AddSingleton<IAuditLogger>(provider =>
                new AuditLogger(
                    provider.GetRequiredService<IConnectionMultiplexer>(),
                    keyFactory.CreateAuditPrefix()));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = keyFactory.CreateGlobalInstancePrefix();
        });


        services.AddSession();
        services.AddResponseCaching();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {

        return services
            .AddScoped<INewsLetterSubscriptionRepository, NewsLetterSubscriptionRepository>();
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
                var fakeContacts = GenerateFakeContacts(500_000, factory);
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
