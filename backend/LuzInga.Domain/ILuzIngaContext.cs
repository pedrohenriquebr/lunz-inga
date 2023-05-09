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
    public void BeginTransaction();
    public Task CommitTransactionAsync();
    public Task RollbackAsync();
}