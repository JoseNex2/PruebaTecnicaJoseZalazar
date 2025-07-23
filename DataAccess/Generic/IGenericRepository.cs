using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using entities.DataContext;
using entities.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteByIdAsync(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ProductsDbContext _context;

        public GenericRepository(ProductsDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(int id, T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteByIdAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
