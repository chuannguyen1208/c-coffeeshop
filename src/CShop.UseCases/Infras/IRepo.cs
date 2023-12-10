using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Infras;
public interface IRepo<TEntity>
    where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetManyAsync(CancellationToken cancellationToken);
}
