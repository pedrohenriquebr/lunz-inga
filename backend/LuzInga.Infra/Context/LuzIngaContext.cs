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
using Microsoft.EntityFrameworkCore.Storage;

public sealed class LuzIngaContext : DbContext, ILuzIngaContext, IUnitOfWork
{
    private IMediator? mediator = null;
    private IDbContextTransaction _transaction;

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
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<SubscriptionId>()
            .HaveConversion<SubscriptionIdValueConverter>();
        configurationBuilder.Properties<SubscriptionStatus>()
            .HaveConversion<SubscriptionStatusValueConverter>();
    }

    public void BeginTransaction()
    {
        this._transaction = Database.BeginTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        await this.SaveChangesAsync(default);
        await this._transaction.CommitAsync();
        await mediator?.DispatchDomainEventsAsync(this);
    }

    public async Task RollbackAsync()
    {
        await this._transaction.RollbackAsync();
    }
}

