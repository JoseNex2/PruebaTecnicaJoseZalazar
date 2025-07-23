using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Utils;
using Contracts;
using entities.Domain;

namespace DataAccess.Generic
{
    internal interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<Result<Product>> CreateAsync(ProductDTO dto);
        Task<Result<Product>> UpdateAsync(int id, ProductDTO dto);
        Task<Result<Product>> DeleteAsync(int id);
    }

    public class ProductService: IProductService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task <Result<IEnumerable<ProductDTO>>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            var productDtos = new List<ProductDTO>();

            foreach (var product in products)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

                productDtos.Add(new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    CategoryName = category?.Name ?? "Sin categoría"
                });
            }

            return Result<IEnumerable<ProductDto>>.Ok(productDtos);
        }
    }
    }
}
