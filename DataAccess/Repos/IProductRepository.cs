using DataAccess.Generic;
using entities.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //IQueryable GetAllAsync(Expression<Func<Product, bool>> condition);
    }

    public class ProductRepository : IProductRepository
    {
        protected readonly IProductRepository _productRepository;

        public ProductRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task CreateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByEntityAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> FindManyAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetAllAsync(Expression<Func<Product, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _productRepository
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
