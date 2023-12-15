using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Infras;
public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork();
}
