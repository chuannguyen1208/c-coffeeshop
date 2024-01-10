namespace CShop.Domain.Repositories;
public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork();
}
