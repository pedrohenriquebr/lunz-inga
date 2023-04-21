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

public class LuzIngaContext : DbContext, ILuzIngaContext
{
    private IMediator mediator;

    public LuzIngaContext(DbContextOptions<LuzIngaContext> options)
        : base(options) { }

    public LuzIngaContext WithMediator(IMediator mediator) {
        this.mediator = mediator;
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

    public async Task<bool> SaveChangesAsync(CancellationToken token = default){
        await this.mediator.DispatchDomainEventsAsync(this);
        var result = await base.SaveChangesAsync(token);
        return true;
    }
}

