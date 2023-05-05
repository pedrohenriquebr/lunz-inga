using LuzInga.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LuzInga.Domain;

public interface ILuzIngaContext
{
    public DbSet<NewsLetterSubscription> NewsLetterSubscription { get; set; }
}


public interface IUnitOfWork 
{
    public Task<bool> SaveChangesAsync(CancellationToken token = default);
    public DatabaseFacade Database { get; }
    void Dispose();
}