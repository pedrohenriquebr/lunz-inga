using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LuzInga.Application.Services;
using LuzInga.Application.Common.CQRS;
using MediatR;
using LuzInga.Domain;
using LuzInga.Domain.Factories;
using LuzInga.Application.Abstractions.Messaging;
using FluentValidation;

namespace LuzInga.Application.Usecases.NewsletterSubscription.SubscribeNewsLetter;

public sealed class SubscribeNewsLetterActionHandler
    : BaseApiCommandHandler<SubscribeNewsLetterCommand>
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
        [FromBody] SubscribeNewsLetterCommand request,
        CancellationToken cancellationToken = default
    )
    {
        await mediator.Send(request);
        bloomFilter.Add(request.Email);

        return NoContent();
    }
}



public sealed class SubscribeNewsletterValidator : AbstractValidator<SubscribeNewsLetterCommand>

{
    public SubscribeNewsletterValidator()
    {
        RuleFor(x => x.Email)
            .NotNull();

        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotNull();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}


public sealed class SubscribeNewsletterHandler : ICommandHandler<SubscribeNewsLetterCommand>
{
    private readonly ILuzIngaContext context;
    private readonly INewsLetterSubscriptionFactory factory;
    public SubscribeNewsletterHandler(ILuzIngaContext contactService, INewsLetterSubscriptionFactory factory)
    {
        this.context = contactService;
        this.factory = factory;
    }

    public async Task Handle(SubscribeNewsLetterCommand request, CancellationToken cancellationToken)
    {
        var newSubscription = factory.CreateSubscription(request.Name, request.Email);
        await context.NewsLetterSubscription.AddAsync(newSubscription);

        await Unit.Task;
    }
}
public sealed class SubscribeNewsLetterCommand : ICommand
{
    public string Email { get; set; }
    public string Name { get; set; }
}
