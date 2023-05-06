using Ardalis.ApiEndpoints;

namespace LuzInga.Application.Common.CQRS;

public abstract class BaseApiQueryHandler <TRequest, TResponse> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithResult<TResponse> 
{
    
}
