using LuzInga.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Application;

public interface ILuzIngaContext
{
    public DbSet<Contact> Contact { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken token = default);
}