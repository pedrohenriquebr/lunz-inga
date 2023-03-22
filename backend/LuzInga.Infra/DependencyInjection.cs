using Bogus;
using LuzInga.Application;
using LuzInga.Domain.Entities;
using LuzInga.Infra.Context;
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

    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        // Add DbContext to dependency injection container
        services.AddDbContext<LuzIngaContext>(
            options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
        );

        // Generate fake Contacts and add to LuzIngaContext
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LuzIngaContext>();
            if (!context.Contact.Any())
            {
                var fakeContacts = GenerateFakeContacts(1_000);
                context.AddRange(fakeContacts);
                context.SaveChanges();
            }
        }

        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<LuzIngaContext>());

        return services;
    }

    private static IEnumerable<Contact> GenerateFakeContacts(int count)
    {
        var faker = new Faker<Contact>()
            .UseSeed(1314159)
            .RuleFor(c => c.Email, f => f.Person.Email)
            .RuleFor(c => c.Name, f => f.Person.FullName);

        return faker.Generate(count);
    }
}
