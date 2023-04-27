using Ardalis.ApiEndpoints;
using LuzInga.Application.Services;
using LuzInga.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.CheckEmail;

public class CheckEmailWithBoomFilterHandler
    : EndpointBaseAsync.WithoutRequest.WithActionResult<CheckEmailQueryResponse>
{

    private readonly IMediator mediator;

    public CheckEmailWithBoomFilterHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(Strings.API_ROUTE_NEWSLETTER_CHECK_EMAIL+"/v2/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in bloom filter first, then checks on the newslettersubscription table on database",
        Description = "Return if exists email",
        OperationId = "NewsLetterSubscription.Checkemail",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult<CheckEmailQueryResponse>> HandleAsync(
        CancellationToken cancellationToken = default
    )
    {
        //todo: validate email format with regex
        var emailRequested = RouteData.Values["email"].ToString();
        var response = await mediator.Send(new CheckEmailWithBloomFilterQuery(emailRequested));
        return Ok(response);
    }
}



public class CheckEmailWithBloomFilterQueryHandler : IRequestHandler<CheckEmailWithBloomFilterQuery, CheckEmailQueryResponse>
{
    private readonly ILuzIngaContext context;
    private readonly IBloomFilter bloomFilter;

    public CheckEmailWithBloomFilterQueryHandler(ILuzIngaContext context, IBloomFilter bloomFilter)
    {
        this.context = context;
        this.bloomFilter = bloomFilter;
    }

    public async Task<CheckEmailQueryResponse> Handle(CheckEmailWithBloomFilterQuery request, CancellationToken cancellationToken)
    {
        if (bloomFilter.MaybeContains(request.Email!))
            return 
                new CheckEmailQueryResponse(
                    await context.NewsLetterSubscription.AnyAsync(
                        x => x.Email == request.Email,
                        cancellationToken
                    )
                );

        return new(false);
    }
}


public sealed record CheckEmailWithBloomFilterQuery (
    string Email
) : IRequest<CheckEmailQueryResponse>;