using LuzInga.Application.Common.CQRS;
using LuzInga.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;

public class ConfirmEmailHandler : CommandHandler<ConfirmEmailCommand>
{
    private readonly IMediator mediator;

    public ConfirmEmailHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost(Strings.API_ROUTE_NEWSLETTER_CONFIRM_EMAIL)]
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


public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly ILuzIngaContext context;

    public ConfirmEmailCommandHandler(ILuzIngaContext context)
    {
        this.context = context;
    }

    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var subscription = context.NewsLetterSubscription.FirstOrDefault(d => d.ConfirmationCode == request.ConfirmationCode);

        if(subscription is null)
            throw new Exception("Subscription not found, review your confirmationCode!");

        subscription.ConfirmEmail();
        await context.SaveChangesAsync();
        await Unit.Task;
    }
}


public class ConfirmEmailCommand : IRequest {
    public string ConfirmationCode { get; set; }
}