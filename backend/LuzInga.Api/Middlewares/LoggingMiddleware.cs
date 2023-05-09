using System.Text;
using LuzInga.Application.Events;
using LuzInga.Application.Services;

namespace LuzInga.Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAuditLogger logger;

        public LoggingMiddleware(RequestDelegate next, IAuditLogger logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            await next(context);
            if(context.Request.Path.Value.ToString().Contains("/check-email/"))
                return;
            await this.logger.LogRecent(new ApplicationAccessedEvent(
                DateTime.UtcNow,
                context.Request.Path.Value,
                context.User?.Identity?.Name ?? "anonymous",
                context.Request.Method,
                context.Connection.RemoteIpAddress?.ToString()
            ));
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
}
