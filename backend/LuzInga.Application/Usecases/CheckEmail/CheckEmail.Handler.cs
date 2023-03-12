using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.CheckEmail;


public class CheckEmailHandler : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<CheckEmailResponse>
{
    [HttpGet("/api/check-email/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in the database",
        Description = "Return if exists email",
        OperationId = "Newsletter.Checkemail",
        Tags = new[] { "CheckEmailEndpoint" })
    ]
    public override async Task<ActionResult<CheckEmailResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(150);

        return Ok();
    }
}