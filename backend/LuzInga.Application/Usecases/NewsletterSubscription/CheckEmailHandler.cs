using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Common.CQRS;
using LuzInga.Application.Services;
using LuzInga.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.CheckEmail;



public class CheckEmailHandler
    : BaseApiQueryHandler<string, CheckEmailQueryResponse>
{
    private readonly IMediator mediator;

    public CheckEmailHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(Strings.API_ROUTE_NEWSLETTER_CHECK_EMAIL+"/{email}")]
    [SwaggerOperation(
        Summary = "Check if emails exists in the newslettersubscription table on database",
        Description = "Return if exists email",
        OperationId = "NewsLetterSubscription.Checkemail",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<CheckEmailQueryResponse> HandleAsync(        
        [FromRoute]
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await mediator.Send(new CheckEmailQuery(email));
    }
}

public class CheckEmailQueryValidator: AbstractValidator<CheckEmailQuery>
{
    
    public CheckEmailQueryValidator()
    {
        RuleFor(d => d.Email)
            .NotNull();
        
        RuleFor(d => d.Email)
            .NotEmpty();
    }
}

public class CheckEmailQueryHandler : IQueryHandler<CheckEmailQuery, CheckEmailQueryResponse>
{

    private readonly ILuzIngaContext context;
    private readonly IBloomFilter bloomFilter;

    public CheckEmailQueryHandler(ILuzIngaContext context, IBloomFilter bloomFilter)
    {
        this.context = context;
        this.bloomFilter = bloomFilter;
    }

    public async Task<CheckEmailQueryResponse> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
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


public sealed record CheckEmailQuery (
    string Email
) : IQuery<CheckEmailQueryResponse>;


public sealed record CheckEmailQueryResponse
(
    bool Exists
);