using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Services;
using MediatR;

namespace LuzInga.Application.Behaviors
{
    public sealed class RecentLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAuditLogger logger;

        public RecentLoggingBehavior(IAuditLogger dbContext)
        {
            this.logger = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            await logger.LogRecent(typeof(TRequest).Name, request, null );
            response = await next();
            await logger.LogRecent(typeof(TRequest).Name, null, response );

            return response;
        }

    }

}