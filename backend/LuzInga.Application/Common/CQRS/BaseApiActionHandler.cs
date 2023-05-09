using Ardalis.ApiEndpoints;

namespace LuzInga.Application.Common.CQRS;

public abstract class BaseApiActionHandler <TRequest, TResponse> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithResult<TResponse> 
{
    
}