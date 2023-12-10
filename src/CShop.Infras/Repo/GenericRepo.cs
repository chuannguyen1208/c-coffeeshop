using CShop.UseCases.Infras;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras.Repo;
internal class GenericRepo<TEntity>(DbContext context) : IRepo<TEntity>
    where TEntity : class
{
    private DbSet<TEntity> _dbSet { get; init; } = context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        _dbSet.Remove(entity!);
    }

    public async Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetManyAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(_dbSet.AsEnumerable());
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }
}
