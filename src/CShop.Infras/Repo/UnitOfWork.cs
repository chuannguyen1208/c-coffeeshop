﻿using CShop.UseCases.Infras;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infras.Repo;
internal class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public IRepo<TEntity> GetRepo<TEntity>() where TEntity : class
    {
        return new GenericRepo<TEntity>(context);
    }

    public void BeginTransaction()
    {
        context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        context.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        context.Database.RollbackTransaction();
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
