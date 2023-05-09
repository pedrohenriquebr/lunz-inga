using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuzInga.Domain.SharedKernel;

namespace LuzInga.Domain.Services;

public interface IRepository<TEntity,TKey>
where TEntity : IAggregateRoot
where TKey : IComparable
{
    public TEntity? GetById(TKey key);
    public Task Save(TEntity entity);
}
