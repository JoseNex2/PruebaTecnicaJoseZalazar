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
        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public ProductsDbContext Context { get; }

        public UnitOfWork(ProductsDbContext context)
        {
            Context = context;
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }
        void IDisposable.Dispose()
        {
            Context.Dispose();
        }
    }
}
