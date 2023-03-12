using Microsoft.AspNetCore.Builder;

namespace LuzInga.Application;

public static class DependencyInjection
{
    
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder  builder)
    {

        return builder;
    }
}