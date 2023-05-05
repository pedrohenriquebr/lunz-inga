using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using LuzInga.Application.Configuration;
using LuzInga.Application.Extensions;
using LuzInga.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace LuzInga.Api.Middlewares
{

    public class CachingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDistributedCache cache;
        private readonly IOptions<RedisConfig> config;
        private readonly IRedisKeyFactory keyFactory;

        public CachingMiddleware(RequestDelegate next, IDistributedCache cache, IOptions<RedisConfig> config, IRedisKeyFactory keyFactory)
        {
            this.next = next;
            this.cache = cache;
            this.config = config;
            this.keyFactory = keyFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var path = context.Request.Path;
            if (!context.Request.Method.Equals(HttpMethods.Get, StringComparison.OrdinalIgnoreCase) 
                || path.ToString().Contains("/check-email/"))
            {
                await next(context);
                return;
            }

            var key = this.keyFactory.CreateCachingKey(path, context.Request.Query);

            // Try to get the cached result from the cache
            var cachedResult = await this.cache.GetStringAsync(key);

            if (cachedResult != null)
            {
                // Return the cached result to the response
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResult);
                return;
            }

            var originalBodyStream = context.Response.Body;
            using (var responseBodyStream = new MemoryStream())
            {
                context.Response.Body = responseBodyStream;

                try
                {
                    await next(context);

                    if(context.Response.Headers.CacheControl.Any()){
                        // Read the response body from the stream
                        var responseContent = await ReadResponseBodyAsync(context.Response);

                        // Cache the response body
                        await this.cache.SetRequestAsync(key, responseContent);

                        // Write the response body to the original stream
                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                    }

                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
                finally
                {
                    // Reset the response body stream
                    context.Response.Body = originalBodyStream;
                }
            }
        }


        public static async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: -1, leaveOpen: true);

            var requestBody = await streamReader.ReadToEndAsync();

            request.Body.Position = 0;

            return requestBody;
        }

        public static async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }

    }


    public sealed record CacheEntry (
        DateTime CreatedAt,
        string Payload 
    );
}