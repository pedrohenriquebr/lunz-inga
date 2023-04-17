using Ardalis.ApiEndpoints;

namespace LuzInga.Application.Common.CQRS;

public abstract class QueryHandler <TRequest, TResponse> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithResult<TResponse> 
{
    
}
