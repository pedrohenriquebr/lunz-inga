using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.Contact.CheckEmail;

public class CheckEmailHandler
    : EndpointBaseAsync.WithoutRequest.WithActionResult<CheckEmailResponse>
{
    private readonly IDbContext context;

    public CheckEmailHandler(IDbContext context)
    {
        this.context = context;
    }

    [HttpGet("/api/contact/check-email/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in the contacts table on database",
        Description = "Return if exists email",
        OperationId = "Contact.Checkemail",
        Tags = new[] { "Contact" }
    )]
    public override async Task<ActionResult<CheckEmailResponse>> HandleAsync(
        CancellationToken cancellationToken = default
    )
    {
        //todo: validate email format with regex
        var emailResquested = RouteData.Values["email"].ToString();

        var existsEmail = await context.Contact.AnyAsync(
            x => x.Email == emailResquested,
            cancellationToken
        );
        await Task.Delay(150);
        return Ok(new CheckEmailResponse(existsEmail));
    }
}
