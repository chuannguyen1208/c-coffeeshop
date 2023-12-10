using CShop.UseCases.Infras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras.Repo;
internal class GenericRepo<TEntity> : IRepo<TEntity>
    where TEntity : class
{
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetManyAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
