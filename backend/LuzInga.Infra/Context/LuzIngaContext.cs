using System.Collections.Concurrent;
namespace LuzInga.Infra.Context;

using System.Threading.Tasks;
using LuzInga.Application;
using LuzInga.Domain.Common;
using LuzInga.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

public class LuzIngaContext : DbContext, ILuzIngaContext
{
    private readonly IMediator mediator;
    public LuzIngaContext(DbContextOptions<LuzIngaContext> options)
        : base(options) { }

    public DbSet<Contact> Contact { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasKey(x => x.Id);

        modelBuilder.Entity<Contact>().Property(x => x.Name).IsRequired();

        modelBuilder.Entity<Contact>().Property(x => x.Email).IsRequired();
    }

    public sealed override Task<int> SaveChangesAsync(CancellationToken token){
        
        var entities  = this.ChangeTracker
                                    .Entries()
                                    .Where(s => s is IAggregateRoot)
                                    .ToList();

        foreach (var entity in entities)
        {
           var events = (IReadOnlyCollection<BaseEvent>)entity.Property("DomainEvents");

           foreach (var @event in events)
           {
                mediator.Publish(@event);
           }
           
        }

        return base.SaveChangesAsync(token);
    }
}

