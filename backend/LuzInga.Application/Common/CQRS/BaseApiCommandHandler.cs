using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace LuzInga.Application.Common.CQRS;

public abstract class BaseApiCommandHandler<TRequest> : EndpointBaseAsync
    .WithRequest<TRequest>
    .WithActionResult
{
    
}
