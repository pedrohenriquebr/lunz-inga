using Ardalis.ApiEndpoints;

namespace LuzInga.Application.Common.CQRS;

public abstract class CommandHandler <TRequest> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithActionResult 
{
    
}
