using Ardalis.ApiEndpoints;
using FluentValidation;
using LuzInga.Application.Abstractions.Messaging;
using LuzInga.Domain;
using LuzInga.Domain.SharedKernel.Exceptions;
using MediatR;

namespace LuzInga.Application.Usecases.NewsletterSubscription.ConfirmEmail;

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