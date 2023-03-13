using Microsoft.AspNetCore.Builder;

namespace LuzInga.Infra;

public static class DependencyInjection
{
    
    public static WebApplicationBuilder AddInfra(this WebApplicationBuilder  builder)
    {

        return builder;
    }
}