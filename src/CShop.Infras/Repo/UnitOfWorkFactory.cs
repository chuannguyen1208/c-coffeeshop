using CShop.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace CShop.Infras.Repo;
internal class UnitOfWorkFactory(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(dbContextFactory.CreateDbContext());
    }
}
