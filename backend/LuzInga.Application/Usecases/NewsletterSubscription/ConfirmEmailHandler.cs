using Hangfire;
using LuzInga.Application.Common.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;


[Route(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
public class ConfirmEmailHandler : BaseApiCommandHandler<string>
{ 

    private readonly IMediator mediator;

    public ConfirmEmailHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("confirm-email/{confirmationCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerOperation(
        Summary = "Confirm email",
        OperationId = "NewsLetterSubscription.ConfirmEmail",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute]
        string confirmationCode, CancellationToken cancellationToken = default)
    {
        
        mediator.Enqueue(new ConfirmEmailCommand(){
            ConfirmationCode = confirmationCode
        });

        return NoContent();
    }
}
