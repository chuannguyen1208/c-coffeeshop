namespace CShop.Domain.Repositories;
public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    Task SaveChangesAsync();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
    IRepo<TEntity> GetRepo<TEntity>() where TEntity : class;
}