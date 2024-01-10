namespace CShop.Domain.Repositories;
public interface IRepo<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Entities { get; }
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetManyAsync(CancellationToken cancellationToken);
}
