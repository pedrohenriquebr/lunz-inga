using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Domain.Entities;
using LuzInga.Domain.Services;
using LuzInga.Domain.ValueObjects;
using LuzInga.Infra.Context;

namespace LuzInga.Infra.Services.Repositories;

public sealed class NewsLetterSubscriptionRepository : INewsLetterSubscriptionRepository
{
    private readonly LuzIngaContext context;

    public NewsLetterSubscriptionRepository(LuzIngaContext context)
    {
        this.context = context;
    }

    public NewsLetterSubscription? GetByConfirmationId(string confirmationCode)
    {
        return context.NewsLetterSubscription.FirstOrDefault(d => d.ConfirmationCode == confirmationCode);
    }

    public NewsLetterSubscription? GetById(SubscriptionId key)
    {
        return context.NewsLetterSubscription.FirstOrDefault(d => d.Id == key);
    }

    public async Task Save(NewsLetterSubscription entity)
    {
        await context.NewsLetterSubscription.AddAsync(entity);
    }
}