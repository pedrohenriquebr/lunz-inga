using Hangfire;
using LuzInga.Application;
using LuzInga.Domain.SharedKernel;
using LuzInga.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Infra;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, LuzIngaContext context )
    {
        var entities = context.ChangeTracker.Entries<IEntity>()
                                        .Where(e => e.State != EntityState.Detached 
                                                    && e.Entity.DomainEvents !=null
                                                    && e.Entity.DomainEvents.Any())
                                        .Select(e => e.Entity)
                                        .ToList();


        var domainEvents = entities
            .SelectMany(d => d.DomainEvents)
            .ToList();

        entities
            .ForEach(e => e.ClearDomainEvents());

        foreach (var @event in domainEvents)
            mediator.EnqueueEvent(@event);
    }
}