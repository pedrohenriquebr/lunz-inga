using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Hangfire;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Application.Common.CQRS;
using LuzInga.Domain;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel.Exceptions;
using LuzInga.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LuzInga.Application.Usecases.NewsletterSubscription;


[Route(Strings.API_BASEURL_NEWSLETTER_SUBSCRIPTION)]
public class RefreshConfirmationTokenHandler
    : BaseApiCommandHandler<Guid>
{

    private readonly IMediator mediator ;

    public RefreshConfirmationTokenHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("{subscriptionid}/refresh-confirmation-code")]
    [SwaggerOperation(
        Summary = "Refresh confirmation code",
        Description = "Return confirmation code for that newsletter subscription, use it if confirmation code was expired.",
        OperationId = "NewsLetterSubscription.RefreshConfirmationToken",
        Tags = new[] { "NewsLetterSubscription" }
    )]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute]
        Guid subscriptionid, 
        CancellationToken cancellationToken = default)
    {
        mediator.Enqueue(new RefreshConfirmationTokenCommand(subscriptionid));

        return NoContent();
    }
}


public sealed record RefreshConfirmationTokenCommand(
    Guid SubscriptionId
) : ICommand;

public sealed class RefreshConfirmationTokenCommandValidator : AbstractValidator<RefreshConfirmationTokenCommand>
{
    public RefreshConfirmationTokenCommandValidator()
    {
        RuleFor(d => d.SubscriptionId)
            .NotNull();


        RuleFor(d => d.SubscriptionId)
            .NotEmpty();
    
    }
}

public sealed class RefreshConfirmationTokenCommandHander : ICommandHandler<RefreshConfirmationTokenCommand>
{
    private readonly INewsLetterSubscriptionRepository repo;
    private readonly ISubscriptionConfirmationCodeFactory tokenProvider;

    public RefreshConfirmationTokenCommandHander(INewsLetterSubscriptionRepository context, ISubscriptionConfirmationCodeFactory tokenProvider)
    {
        this.repo = context;
        this.tokenProvider = tokenProvider;
    }

    public async Task Handle(RefreshConfirmationTokenCommand request, CancellationToken cancellationToken)
    {
        var subscription = repo.GetById(request.SubscriptionId);

        if(subscription is null)
            throw new GlobalApplicationException(ApplicationExceptionType.Validation, "Subscription not found.");
        
        var newToken = tokenProvider.Generate(request.SubscriptionId);

        subscription.RefreshConfirmationCode(newToken);

        await Unit.Task;
    }
}


