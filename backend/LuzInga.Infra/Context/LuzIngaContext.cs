using System.Collections.Concurrent;
namespace LuzInga.Infra.Context;

using System.Threading.Tasks;
using LuzInga.Application;
using LuzInga.Domain;
using LuzInga.Domain.SharedKernel;
using LuzInga.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Reflection;
using LuzInga.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LuzInga.Infra.Context.ValueConverters;

public sealed class LuzIngaContext : DbContext, ILuzIngaContext, IUnitOfWork
{
    private IMediator? mediator = null;

    public LuzIngaContext(DbContextOptions<LuzIngaContext> options)
        : base(options) { }

    public LuzIngaContext WithMediator(IMediator mediator)
    {
        this.mediator = mediator;
        return this;
    }

    public LuzIngaContext DisableMediator()
    {
        this.mediator = null;
        return this;
    }

    public DbSet<NewsLetterSubscription> NewsLetterSubscription { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsLetterSubscription>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<NewsLetterSubscription>()
            .Property(x => x.Id)
            .HasColumnName("NewsLetterSubscriptionId");

        modelBuilder.Entity<NewsLetterSubscription>().Property(x => x.Name).IsRequired();

        modelBuilder.Entity<NewsLetterSubscription>().Property(x => x.Email).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<SubscriptionId>()
            .HaveConversion<SubscriptionIdValueConverter>();
        base.ConfigureConventions(configurationBuilder);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken token = default)
    {
        var result = await base.SaveChangesAsync(token);
        await mediator?.DispatchDomainEventsAsync(this);
        return true;
    }
}

