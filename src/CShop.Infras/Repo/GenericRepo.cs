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
    private DbSet<TEntity> DbSet { get; init; } = context.Set<TEntity>();

    public IQueryable<TEntity> Entities => DbSet;

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync([id], cancellationToken: cancellationToken);
        DbSet.Remove(entity!);
    }

    public async Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync([id], cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetManyAsync(CancellationToken cancellationToken)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        DbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }
}
