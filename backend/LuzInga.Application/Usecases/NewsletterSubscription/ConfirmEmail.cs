using Ardalis.ApiEndpoints;
using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Common.CQRS;
using LuzInga.Domain;
using LuzInga.Domain.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;

public class ConfirmEmailHandler : EndpointBaseAsync
    .WithRequest<ConfirmEmailCommand>
    .WithActionResult 
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

public sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
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
            throw new GlobalApplicationException(ApplicationExceptionType.Validation, "Subscription not found, review your confirmationCode!");

        subscription.ConfirmEmail();
        await Unit.Task;
    }
}


public sealed class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{

    public ConfirmEmailCommandValidator()
    {
        RuleFor(d => d.ConfirmationCode)
            .NotNull();

        RuleFor(d => d.ConfirmationCode)
            .NotEmpty();
    }
    
}


public class ConfirmEmailCommand : ICommand {
    public string ConfirmationCode { get; set; }
}