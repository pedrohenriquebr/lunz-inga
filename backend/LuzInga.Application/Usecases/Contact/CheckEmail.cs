using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.Contact.CheckEmail;



public class CheckEmailHandler
    : EndpointBaseAsync.WithoutRequest.WithActionResult<CheckEmailQueryResponse>
{
    private readonly IMediator mediator;

    public CheckEmailHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("/api/contact/check-email/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in the contacts table on database",
        Description = "Return if exists email",
        OperationId = "Contact.Checkemail",
        Tags = new[] { "Contact" }
    )]
    public override async Task<ActionResult<CheckEmailQueryResponse>> HandleAsync(
        CancellationToken cancellationToken = default
    )
    {
        var emailRequested = RouteData.Values["email"].ToString();
        var response = await mediator.Send(new CheckEmailQuery(emailRequested));
        return Ok(response);
    }
}


public class CheckEmailQueryHandler : IRequestHandler<CheckEmailQuery, CheckEmailQueryResponse>
{

    private readonly ILuzIngaContext context;

    public CheckEmailQueryHandler(ILuzIngaContext context)
    {
        this.context = context;
    }

    public async Task<CheckEmailQueryResponse> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
    {
        var existsEmail = await context.Contact.AnyAsync(
            x => x.Email == request.Email,
            cancellationToken
        );

        return new CheckEmailQueryResponse(existsEmail);
    }
}


public sealed record CheckEmailQuery (
    string Email
) : IRequest<CheckEmailQueryResponse>;


public sealed record CheckEmailQueryResponse
(
    bool Exists
);