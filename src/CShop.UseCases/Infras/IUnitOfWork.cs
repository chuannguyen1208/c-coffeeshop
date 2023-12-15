using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Infras;
public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    Task SaveChangesAsync();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
    IRepo<TEntity> GetRepo<TEntity>() where TEntity : class;
}
