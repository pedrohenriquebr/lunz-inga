using LuzInga.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuzInga.Application;

public interface IDbContext
{
    public DbSet<Contact> Contact { get; set; }
}
