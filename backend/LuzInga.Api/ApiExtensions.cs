using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Api.Middlewares;
using LuzInga.Domain.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace LuzInga.Api
{
    public static class ApiExtensions
    {
        public static IApplicationBuilder UseResponseCachingExtended(this WebApplication app)
            => app.UseMiddleware<CachingMiddleware>();

        public static IApplicationBuilder UseLogging(this WebApplication app)
            => app.UseMiddleware<LoggingMiddleware>();


        public static IApplicationBuilder UseGlobalExceptionHandler(this WebApplication app)
            => app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                var originalException = exceptionHandlerPathFeature.Error;

                if (originalException is GlobalApplicationException globalException)
                {
                    context.Response.StatusCode = (int)globalException.Type;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Type = globalException.Type.ToString(),
                        Message = globalException.Message,
                        ErrorCode = globalException.Code,
                        Errors =  globalException.Errors
                    });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Type = ApplicationExceptionType.Application.ToString(),
                        Message = originalException.Message
                    });
                }
            }));
    }
}