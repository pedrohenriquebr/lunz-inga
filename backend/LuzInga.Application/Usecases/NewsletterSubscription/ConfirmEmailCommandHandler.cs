using Ardalis.ApiEndpoints;
using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Domain;
using LuzInga.Domain.Services;
using LuzInga.Domain.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;


public sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    private readonly INewsLetterSubscriptionRepository repo;

    public ConfirmEmailCommandHandler(INewsLetterSubscriptionRepository context)
    {
        this.repo = context;
    }

    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var subscription = repo.GetByConfirmationId(request.ConfirmationCode);

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