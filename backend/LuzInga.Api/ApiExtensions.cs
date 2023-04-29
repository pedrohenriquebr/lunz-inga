using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Api.Middlewares;
namespace LuzInga.Api
{
    public static class ApiExtensions
    {
        public static IApplicationBuilder UseResponseCachingEx(this WebApplication app)
            => app.UseMiddleware<CachingMiddleware>();

        public static IApplicationBuilder UseLogging(this WebApplication app)
            => app.UseMiddleware<LoggingMiddleware>();


    }
}