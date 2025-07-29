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
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductsDbContext _context;

        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Category> Categories { get; }

        public UnitOfWork(ProductsDbContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Categories = new GenericRepository<Category>(_context);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
