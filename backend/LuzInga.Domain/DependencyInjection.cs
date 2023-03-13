using Microsoft.AspNetCore.Builder;

namespace LuzInga.Domain;

public static class DependencyInjection
{
    
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder  builder)
    {

        return builder;
    }
}