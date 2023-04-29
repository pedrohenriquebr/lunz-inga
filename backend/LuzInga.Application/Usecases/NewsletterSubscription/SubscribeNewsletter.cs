using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LuzInga.Application.Services;
using LuzInga.Application.Common.CQRS;
using MediatR;
using LuzInga.Domain;
using LuzInga.Domain.Factories;

namespace LuzInga.Application.Usecases.NewsletterSubscription.SubscribeNewsLetter;

public class SubscribeNewsLetterActionHandler
    : CommandHandler<SubscribeNewsLetterRequest>
{
    private readonly IBloomFilter bloomFilter;
    private readonly IMediator mediator;

    public SubscribeNewsLetterActionHandler(IBloomFilter bloomFilter, IMediator mediator)
    {
        this.bloomFilter = bloomFilter;
        this.mediator = mediator;
    }

    [HttpPost(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(
        Summary = "Subscribe to newsletter",
        OperationId = "NewsLetterSubscription.Subscribe",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult> HandleAsync(
        [FromBody] SubscribeNewsLetterRequest request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
                return BadRequest();
            
            await mediator.Send(request);
            bloomFilter.Add(request.Email);
        }
        catch (Exception ex) { 
            return StatusCode(StatusCodes.Status500InternalServerError, new {
                error = ex.Message,
                details = ex.StackTrace
            });
        }

        return NoContent();
    }
}

public sealed class SubscribeNewsLetterRequest : IRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
}


public class SubscribeNewsletterHandler : IRequestHandler<SubscribeNewsLetterRequest>
{
    private readonly ILuzIngaContext context;
    private readonly INewsLetterSubscriptionFactory factory;
    public SubscribeNewsletterHandler(ILuzIngaContext contactService, INewsLetterSubscriptionFactory factory)
    {
        this.context = contactService;
        this.factory = factory;
    }

    public async Task Handle(SubscribeNewsLetterRequest request, CancellationToken cancellationToken)
    {
        var newSubscription = factory.CreateSubscription(request.Name, request.Email);
        await context.NewsLetterSubscription.AddAsync(newSubscription);
        await context.SaveChangesAsync();

        await Unit.Task;
    }
}
