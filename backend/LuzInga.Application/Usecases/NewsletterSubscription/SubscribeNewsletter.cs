using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LuzInga.Application.Services;
using LuzInga.Application.Common.CQRS;
using MediatR;
using LuzInga.Domain;
using NewsletterSubscriptionEntity = LuzInga.Domain.Entities.NewsLetterSubscription;  
namespace LuzInga.Application.Usecases.Contact.AddContact;

public class SubscribeNewsLetterActionHandler
    : ActionHandler<SubscribeNewsLetterRequest, ActionResult<SubscribeNewsletterResponse>>
{
    private readonly IBloomFilter bloomFilter;
    private readonly IMediator mediator;

    public SubscribeNewsLetterActionHandler(IBloomFilter bloomFilter, IMediator mediator)
    {
        this.bloomFilter = bloomFilter;
        this.mediator = mediator;
    }

    [HttpPost(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerOperation(
        Summary = "Subscribe to newsletter",
        OperationId = "NewsLetterSubscription.Subscribe",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult<SubscribeNewsletterResponse>> HandleAsync(
        [FromBody] SubscribeNewsLetterRequest request,
        CancellationToken cancellationToken = default
    )
    {

        SubscribeNewsletterResponse? response = null;
        try
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
                return BadRequest();
            
            response = await mediator.Send(request);
            bloomFilter.Add(request.Email);
        }
        catch (Exception ex) { 
            return StatusCode(StatusCodes.Status500InternalServerError, new {
                error = ex.Message,
                details = ex.StackTrace
            });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            response
        );
    }
}

public sealed class SubscribeNewsLetterRequest : IRequest<SubscribeNewsletterResponse>
{
    public string Email { get; set; }
    public string Name { get; set; }
}


public sealed class SubscribeNewsletterResponse
{
    public int NewsletterSubscriptionId { get; set; }
}


public class SubscribeNewsletterHandler : IRequestHandler<SubscribeNewsLetterRequest, SubscribeNewsletterResponse>
{
    private readonly ILuzIngaContext context;
    public SubscribeNewsletterHandler(ILuzIngaContext contactService)
    {
        this.context = contactService;
    }

    public async Task<SubscribeNewsletterResponse> Handle(SubscribeNewsLetterRequest request, CancellationToken cancellationToken)
    {
        var newSubscription = NewsletterSubscriptionEntity.Create(request.Email, request.Name);
        await context.NewsLetterSubscription.AddAsync(newSubscription);
        await context.SaveChangesAsync();
        
        return new SubscribeNewsletterResponse(){
            NewsletterSubscriptionId = newSubscription.Id
        };
    }
}
