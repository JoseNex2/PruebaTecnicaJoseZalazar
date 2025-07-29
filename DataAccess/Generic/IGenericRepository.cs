using entities.DataContext;
using entities.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync(Expression<Func<T, bool>>? condition = null);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteByEntityAsync(T entity);
        Task<Product?> GetByIdWithCategoryAsync(int id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ProductsDbContext _context;

        public GenericRepository(ProductsDbContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetAllAsync(Expression<Func<T, bool>>? condition = null)
        {
            var query = _context.Set<T>().AsQueryable(); 
            
            if(condition != null)
            {
                return query.Where(condition);
            }
            return query;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> condition)
        {
            return await _context.Set<T>().Where(condition).ToListAsync();
        }
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteByEntityAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
