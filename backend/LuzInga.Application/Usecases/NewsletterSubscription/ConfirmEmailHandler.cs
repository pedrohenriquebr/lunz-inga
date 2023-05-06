using LuzInga.Application.Common.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;


[Route(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
public class ConfirmEmailHandler : BaseApiCommandHandler<ConfirmEmailCommand>
{
    private readonly IMediator mediator;

    public ConfirmEmailHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(
        Summary = "Confirm email",
        OperationId = "NewsLetterSubscription.ConfirmEmail",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult> HandleAsync(
        [FromBody]
        ConfirmEmailCommand request, CancellationToken cancellationToken = default)
    {
        
        await this.mediator.Send<ConfirmEmailCommand>(request);

        return NoContent();
    }
}
