using LuzInga.Application;
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

        services.AddScoped<IDbContext,LuzIngaContext>();

        return services;
    }
}
