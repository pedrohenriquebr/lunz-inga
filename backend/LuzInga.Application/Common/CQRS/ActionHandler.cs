using Ardalis.ApiEndpoints;

namespace LuzInga.Application.Common.CQRS;

public abstract class ActionHandler <TRequest, TResponse> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithResult<TResponse> 
{
    
}