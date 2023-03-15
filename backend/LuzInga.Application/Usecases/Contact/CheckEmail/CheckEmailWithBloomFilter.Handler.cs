using Ardalis.ApiEndpoints;
using LuzInga.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.Contact.CheckEmail;

public class CheckEmailWithBoomFilterHandler
    : EndpointBaseAsync.WithoutRequest.WithActionResult<CheckEmailResponse>
{
    private readonly IDbContext context;
    private readonly IBloomFilter bloomFilter;

    public CheckEmailWithBoomFilterHandler(IDbContext context, IBloomFilter filter)
    {
        this.context = context;
        this.bloomFilter = filter;
    }

    [HttpGet("/api/contact/check-email/v2/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in bloom filter first, then checks on the contacts table on database",
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

        if (bloomFilter.MaybeContains(emailResquested!))
            return Ok(
                new CheckEmailResponse(
                    await context.Contact.AnyAsync(
                        x => x.Email == emailResquested,
                        cancellationToken
                    )
                )
            );

        return Ok(new CheckEmailResponse(false));
    }
}
