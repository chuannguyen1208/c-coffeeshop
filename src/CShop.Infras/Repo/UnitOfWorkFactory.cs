using CShop.UseCases.Infras;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras.Repo;
internal class UnitOfWorkFactory(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(dbContextFactory.CreateDbContext());
    }
}
