using LuzInga.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Domain;

public interface ILuzIngaContext
{
    public DbSet<NewsLetterSubscription> NewsLetterSubscription { get; set; }
    public Task<bool> SaveChangesAsync(CancellationToken token = default);
}