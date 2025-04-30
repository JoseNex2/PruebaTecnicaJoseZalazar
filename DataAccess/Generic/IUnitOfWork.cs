using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities.DataContext;
using entities.Domain;

namespace DataAccess.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        ProductsDbContext Context { get; }
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public ProductsDbContext Context { get; }

        public UnitOfWork(ProductsDbContext context)
        {
            Context = context;
        }

        void IUnitOfWork.Commit()
        {
            Context.SaveChanges();
        }
        void IDisposable.Dispose()
        {
            Context.Dispose();
        }
    }
}
